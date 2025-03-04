/******************************************************************************

    USB Host Driver

This file provides the hardware interface for a USB Embedded Host application.
Most applications will not make direct use of the functions in this file.
Instead, one or more client driver files should also be included in the project
to support the devices that will be attached to the host.  Application
interface will be through the client drivers.

Note: USB interrupts are cleared by writing a "1" to the interrupt flag.  This
means that read-modify-write instructions cannot be used to clear the flag.  A
bit manipulation instruction, such as "U1OTGIRbits.T1MSECIF = 1;" will read the
value of the U1OTGIR register, set the T1MSECIF bit in that value to "1", and
then write that value back to U1OTGIR.  If U1OTGIR had any other flags set,
those flags are written back as "1", which will clear those flags.  To avoid
this issue, a constant value must be written to U1OTGIR where only the interrupt
flag in question is set, such as "U1OTGIR = USB_INTERRUPT_T1MSECIF;", where
USB_INTERRUPT_T1MSECIF equals 0x40.

*******************************************************************************/
//DOM-IGNORE-BEGIN
/******************************************************************************

* File Name:       usb_host.c
* Dependencies:    None
* Processor:       PIC24/dsPIC30/dsPIC33/PIC32MX
* Compiler:        C30 v2.01/C32 v0.00.18
* Company:         Microchip Technology, Inc.

Software License Agreement

The software supplied herewith by Microchip Technology Incorporated
(the �Company�) for its PICmicro� Microcontroller is intended and
supplied to you, the Company�s customer, for use solely and
exclusively on Microchip PICmicro Microcontroller products. The
software is owned by the Company and/or its supplier, and is
protected under applicable copyright laws. All rights are reserved.
Any use in violation of the foregoing restrictions may subject the
user to criminal sanctions under applicable laws, as well as to
civil liability for the breach of the terms and conditions of this
license.

THIS SOFTWARE IS PROVIDED IN AN �AS IS� CONDITION. NO WARRANTIES,
WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED
TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,
IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR
CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.

Author          Date    Comments
--------------------------------------------------------------------------------
KO/BC       15-Oct-2007 First release

*******************************************************************************/

#include <stdlib.h>
#include <string.h>
#include "GenericTypeDefs.h"
#include "usb.h"
#include "usb_host_local.h"
#include "usb_hal_local.h"
#include "HardwareProfile.h"

#if defined( USB_ENABLE_TRANSFER_EVENT )
    #include "struct_queue.h"
#endif

// *****************************************************************************
// Low Level Functionality Configurations.

//#define DEBUG_MODE
#ifdef DEBUG_MODE
    #include "uart2.h"
#endif

// If the TPL includes an entry specifying a VID of 0xFFFF and a PID of 0xFFFF,
// the specified client driver will be used for any device that attaches.  This
// can be useful for debugging or for providing generic charging functionality.
#define ALLOW_GLOBAL_VID_AND_PID

// If we allow multiple control transactions during a frame and a NAK is
// generated, we don't get TRNIF.  So we will allow only one control transaction
// per frame.
#define ONE_CONTROL_TRANSACTION_PER_FRAME

// This definition allow Bulk transfers to take all of the remaining bandwidth
// of a frame.
#define ALLOW_MULTIPLE_BULK_TRANSACTIONS_PER_FRAME

// If this is defined, then we will repeat a NAK'd request in the same frame.
// Otherwise, we will wait until the next frame to repeat the request.  Some
// mass storage devices require the host to wait until the next frame to
// repeat the request.
//#define ALLOW_MULTIPLE_NAKS_PER_FRAME

//#define USE_MANUAL_DETACH_DETECT


//******************************************************************************
//******************************************************************************
// Section: Host Global Variables
//******************************************************************************
//******************************************************************************

// When using the PIC32, ping pong mode must be set to FULL.
#if defined (__PIC32MX__)
    #if (USB_PING_PONG_MODE != USB_PING_PONG__FULL_PING_PONG)
        #undef USB_PING_PONG_MODE
        #define USB_PING_PONG_MODE USB_PING_PONG__FULL_PING_PONG
    #endif
#endif

#if (USB_PING_PONG_MODE == USB_PING_PONG__NO_PING_PONG) || (USB_PING_PONG_MODE == USB_PING_PONG__ALL_BUT_EP0)
    #if !defined(USB_SUPPORT_OTG)
    static BDT_ENTRY __attribute__ ((aligned(512)))    BDT[2];
    #endif
    #define BDT_IN                                  (&BDT[0])           // EP0 IN Buffer Descriptor
    #define BDT_OUT                                 (&BDT[1])           // EP0 OUT Buffer Descriptor
#elif (USB_PING_PONG_MODE == USB_PING_PONG__EP0_OUT_ONLY)
    #if !defined(USB_SUPPORT_OTG)
    static BDT_ENTRY __attribute__ ((aligned(512)))    BDT[3];
    #endif
    #define BDT_IN                                  (&BDT[0])           // EP0 IN Buffer Descriptor
    #define BDT_OUT                                 (&BDT[1])           // EP0 OUT Even Buffer Descriptor
    #define BDT_OUT_ODD                             (&BDT[2])           // EP0 OUT Odd Buffer Descriptor
#elif (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG)
    #if !defined(USB_SUPPORT_OTG)
    static BDT_ENTRY __attribute__ ((aligned(512)))    BDT[4];
    #endif
    #define BDT_IN                                  (&BDT[0])           // EP0 IN Even Buffer Descriptor
    #define BDT_IN_ODD                              (&BDT[1])           // EP0 IN Odd Buffer Descriptor
    #define BDT_OUT                                 (&BDT[2])           // EP0 OUT Even Buffer Descriptor
    #define BDT_OUT_ODD                             (&BDT[3])           // EP0 OUT Odd Buffer Descriptor
#endif

static BYTE                          countConfigurations;                        // Count the Configuration Descriptors read during enumeration.
static BYTE                          numCommandTries;                            // The number of times the current command has been tried.
static BYTE                          numEnumerationTries;                        // The number of times enumeration has been attempted on the attached device.
static volatile WORD                 numTimerInterrupts;                         // The number of milliseconds elapsed during the current waiting period.
static volatile USB_ENDPOINT_INFO   *pCurrentEndpoint;                           // Pointer to the endpoint currently performing a transfer.
static USB_CONFIGURATION            *pConfigurationDescriptorList       = NULL;  // Pointer to the list of Donfiguration Descriptors of the attached device.
BYTE                                *pCurrentConfigurationDescriptor    = NULL;  // Pointer to the current configuration descriptor of the attached device.
BYTE                                *pDeviceDescriptor                  = NULL;  // Pointer to the Device Descriptor of the attached device.
static USB_ENDPOINT_INFO            *pEndpointList                      = NULL;  // List of endpoints on the attached device.
static BYTE                         *pEP0Data                           = NULL;  // A data buffer for use by EP0.
static USB_INTERFACE_INFO           *pInterfaceList                     = NULL;  // List of interfaces on the attached device.
static USB_BUS_INFO                  usbBusInfo;                                 // Information about the USB bus.
static USB_DEVICE_INFO               usbDeviceInfo;                              // A collection of information about the attached device.
#if defined( USB_ENABLE_TRANSFER_EVENT )
    static USB_EVENT_QUEUE           usbEventQueue;                              // Queue of USB events used to synchronize ISR to main tasks loop.
#endif
static volatile WORD                 usbHostState;                               // State machine state of the attached device.
static volatile WORD                 usbOverrideHostState;                       // Next state machine state, when set by interrupt processing.
static USB_ROOT_HUB_INFO             usbRootHubInfo;                             // Information about a specific port.

#ifdef ENABLE_STATE_TRACE   // Debug trace support
    static WORD prevHostState;
#endif


// *****************************************************************************
// *****************************************************************************
// Section: Application Callable Functions
// *****************************************************************************
// *****************************************************************************

/****************************************************************************
  Function:
    BYTE USBHostClearEndpointErrors( BYTE deviceAddress, BYTE endpoint )

  Summary:
    This function clears an endpoint's internal error condition.

  Description:
    This function is called to clear the internal error condition of a device's
    endpoint.  It should be called after the application has dealt with the
    error condition on the device.  This routine clears internal status only;
    it does not interact with the device.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Address of device
    BYTE endpoint       - Endpoint to clear error condition

  Return Values:
    USB_SUCCESS             - Errors cleared
    USB_UNKNOWN_DEVICE      - Device not found
    USB_ENDPOINT_NOT_FOUND  - Specified endpoint not found

  Remarks:
    None
  ***************************************************************************/

BYTE USBHostClearEndpointErrors( BYTE deviceAddress, BYTE endpoint )
{
    USB_ENDPOINT_INFO *ep;

    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    ep = pEndpointList;
    while (ep != NULL)
    {
        if (ep->bEndpointAddress == endpoint)
        {
            ep->status.bfStalled    = 0;
            ep->status.bfError      = 0;

            return USB_SUCCESS;
        }
        ep = ep->next;
    }
    return USB_ENDPOINT_NOT_FOUND;
}


/****************************************************************************
  Function:
    BYTE USBHostDeviceRequest( BYTE deviceAddress, BYTE bmRequestType,
                    BYTE bRequest, WORD wValue, WORD wIndex, WORD wLength,
                    BYTE *data, BYTE dataDirection )

  Summary:
    This function sends a standard device request to the attached device.

  Description:
    This function sends a standard device request to the attached device.
    The user must pass in the parameters of the device request.  If there is
    input or output data associated with the request, a pointer to the data
    must be provided.  The direction of the associated data (input or output)
    must also be indicated.

    This function does no special processing in regards to the request except
    for three requests.  If SET INTERFACE is sent, then DTS is reset for all
    endpoints.  If CLEAR FEATURE (ENDPOINT HALT) is sent, then DTS is reset
    for that endpoint.

    If the application wishes to change the device configuration, it should
    use the function USBHostSetDeviceConfiguration() rather than this function
    with the SET CONFIGURATION request, since endpoint definitions may
    change.

  Precondition:
    The host state machine should be in the running state, and no reads or
    writes to EP0 should be in progress.

  Parameters:
    BYTE deviceAddress  - Device address
    BYTE bmRequestType  - The request type as defined by the USB
                            specification.
    BYTE bRequest       - The request as defined by the USB specification.
    WORD wValue         - The value for the request as defined by the USB
                            specification.
    WORD wIndex         - The index for the request as defined by the USB
                            specification.
    WORD wLength        - The data length for the request as defined by the
                            USB specification.
    BYTE *data          - Pointer to the data for the request.
    BYTE dataDirection  - USB_DEVICE_REQUEST_SET or USB_DEVICE_REQUEST_GET

  Return Values:
    USB_SUCCESS                 - Request processing started
    USB_UNKNOWN_DEVICE          - Device not found
    USB_INVALID_STATE           - The host must be in a normal running state
                                    to do this request
    USB_ENDPOINT_BUSY           - A read or write is already in progress
    USB_ILLEGAL_REQUEST         - SET CONFIGURATION cannot be performed with
                                    this function.

  Remarks:
    DTS reset is done before the command is issued.
  ***************************************************************************/

BYTE USBHostDeviceRequest( BYTE deviceAddress, BYTE bmRequestType, BYTE bRequest,
            WORD wValue, WORD wIndex, WORD wLength, BYTE *data, BYTE dataDirection )
{
    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    // If we are not in a normal user running state, we cannot do this.
    if ((usbHostState & STATE_MASK) != STATE_RUNNING)
    {
        return USB_INVALID_STATE;
    }

    // Make sure no other reads or writes on EP0 are in progress.
    if (!pEndpointList->status.bfTransferComplete)
    {
        return USB_ENDPOINT_BUSY;
    }

    // We can't do a SET CONFIGURATION here.  Must use USBHostSetDeviceConfiguration().
    // ***** Some USB classes need to be able to do this, so we'll remove
    // the constraint.
//    if (bRequest == USB_REQUEST_SET_CONFIGURATION)
//    {
//        return USB_ILLEGAL_REQUEST;
//    }

    // If the user is doing a SET INTERFACE, we must reset DATA0 for all endpoints.
    if (bRequest == USB_REQUEST_SET_INTERFACE)
    {
        _USB_ResetDATA0( 0 );
    }

    // If the user is doing a CLEAR FEATURE(ENDPOINT_HALT), we must reset DATA0 for that endpoint.
    if ((bRequest == USB_REQUEST_CLEAR_FEATURE) && (wValue == USB_FEATURE_ENDPOINT_HALT))
    {
        _USB_ResetDATA0( (BYTE)wIndex );
    }

    // Set up the control packet.
    pEP0Data[0] = bmRequestType;
    pEP0Data[1] = bRequest;
    pEP0Data[2] = wValue & 0xFF;
    pEP0Data[3] = (wValue >> 8) & 0xFF;
    pEP0Data[4] = wIndex & 0xFF;
    pEP0Data[5] = (wIndex >> 8) & 0xFF;
    pEP0Data[6] = wLength & 0xFF;
    pEP0Data[7] = (wLength >> 8) & 0xFF;

    if (dataDirection == USB_DEVICE_REQUEST_SET)
    {
        // We are doing a SET command that requires data be sent.
        _USB_InitControlWrite( pEndpointList, pEP0Data,8, data, wLength );
    }
    else
    {
        // We are doing a GET request.
        _USB_InitControlRead( pEndpointList, pEP0Data, 8, data, wLength );
    }

    return USB_SUCCESS;
}

/****************************************************************************
  Function:
    BOOL    USBHostDeviceSpecificClientDriver( BYTE deviceAddress )

  Summary:
    This function indicates if the specified device has explicit client
    driver support specified in the TPL.

  Description:
    This function indicates if the specified device has explicit client
    driver support specified in the TPL.  It is used in client drivers'
    USB_CLIENT_INIT routines to indicate that the client driver should be
    used even though the class, subclass, and protocol values may not match
    those normally required by the class.  For example, some printing devices
    do not fulfill all of the requirements of the printer class, so their
    class, subclass, and protocol fields indicate a custom driver rather than
    the printer class.  But the printer class driver can still be used, with
    minor limitations.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Address of device

  Return Values:
    TRUE    - This device is listed in the TPL by VID andPID, and has explicit
                client driver support.
    FALSE   - This device is not listed in the TPL by VID and PID.

  Remarks:
    This function is used so client drivers can allow certain
    devices to enumerate.  For example, some printer devices indicate a custom
    class rather than the printer class, even though the device has only minor
    limitations from the full printer class.   The printer client driver will
    fail to initialize the device if it does not indicate printer class support
    in its interface descriptor.  The printer client driver could allow any
    device with an interface that matches the printer class endpoint
    configuration, but both printer and mass storage devices utilize one bulk
    IN and one bulk OUT endpoint.  So a mass storage device would be
    erroneously initialized as a printer device.  This function allows a
    client driver to know that the client driver support was specified
    explicitly in the TPL, so for this particular device only, the class,
    subclass, and protocol fields can be safely ignored.
  ***************************************************************************/

BOOL    USBHostDeviceSpecificClientDriver( BYTE deviceAddress )
{
    return usbDeviceInfo.flags.bfUseDeviceClientDriver;
}


/****************************************************************************
  Function:
    BYTE USBHostDeviceStatus( BYTE deviceAddress )

  Summary:
    This function returns the current status of a device.

  Description:
    This function returns the current status of a device.  If the device is
    in a holding state due to an error, the error is returned.

  Preconditions:
    None

  Parameters:
    BYTE deviceAddress  - Device address

  Return Values:
    USB_DEVICE_ATTACHED                 - Device is attached and running
    USB_DEVICE_DETACHED                 - No device is attached
    USB_DEVICE_ENUMERATING              - Device is enumerating
    USB_HOLDING_OUT_OF_MEMORY           - Not enough heap space available
    USB_HOLDING_UNSUPPORTED_DEVICE      - Invalid configuration or
                                            unsupported class
    USB_HOLDING_UNSUPPORTED_HUB         - Hubs are not supported
    USB_HOLDING_INVALID_CONFIGURATION   - Invalid configuration requested
    USB_HOLDING_PROCESSING_CAPACITY     - Processing requirement excessive
    USB_HOLDING_POWER_REQUIREMENT       - Power requirement excessive
    USB_HOLDING_CLIENT_INIT_ERROR       - Client driver failed to initialize
    USB_DEVICE_SUSPENDED                - Device is suspended
    Other                               - Device is holding in an error
                                            state. The return value
                                            indicates the error.

  Remarks:
    None
  ***************************************************************************/

BYTE USBHostDeviceStatus( BYTE deviceAddress )
{
    if ((usbHostState & STATE_MASK) == STATE_DETACHED)
    {
        return USB_DEVICE_DETACHED;
    }

    if ((usbHostState & STATE_MASK) == STATE_RUNNING)
    {
        if ((usbHostState & SUBSTATE_MASK) == SUBSTATE_SUSPEND_AND_RESUME)
        {
            return USB_DEVICE_SUSPENDED;
        }
        else
        {
            return USB_DEVICE_ATTACHED;
        }
    }

    if ((usbHostState & STATE_MASK) == STATE_HOLDING)
    {
        return usbDeviceInfo.errorCode;
    }

    return USB_DEVICE_ENUMERATING;
}

/****************************************************************************
  Function:
    BOOL USBHostInit(  unsigned long flags  )

  Summary:
    This function initializes the variables of the USB host stack.

  Description:
    This function initializes the variables of the USB host stack.  It does
    not initialize the hardware.  The peripheral itself is initialized in one
    of the state machine states.  Therefore, USBHostTasks() should be called
    soon after this function.

  Precondition:
    None

  Parameters:
    flags - reserved

  Return Values:
    TRUE  - Initialization successful
    FALSE - Could not allocate memory.

  Remarks:
    If the endpoint list is empty, an entry is created in the endpoint list
    for EP0.  If the list is not empty, free all allocated memory other than
    the EP0 node.  This allows the routine to be called multiple times by the
    application.
  ***************************************************************************/

BOOL USBHostInit(  unsigned long flags  )
{
    // Allocate space for Endpoint 0.  We will initialize it in the state machine,
    // so we can reinitialize when another device connects.  If the Endpoint 0
    // node already exists, free all other allocated memory.
    if (pEndpointList == NULL)
    {
        if ((pEndpointList = (USB_ENDPOINT_INFO*)malloc( sizeof(USB_ENDPOINT_INFO) )) == NULL)
        {
            #ifdef DEBUG_MODE
                UART2PrintString( "HOST: Cannot allocate for endpoint list.\r\n" );
            #endif
            //return USB_MEMORY_ALLOCATION_ERROR;
            return FALSE;
        }
        pEndpointList->next = NULL;
    }
    else
    {
        _USB_FreeMemory();
    }

    // Initialize other variables.
    pCurrentEndpoint                        = pEndpointList;
    usbHostState                            = STATE_DETACHED;
    usbOverrideHostState                    = NO_STATE;
    usbDeviceInfo.deviceAddressAndSpeed     = 0;
    usbDeviceInfo.deviceAddress             = 0;
    usbRootHubInfo.flags.bPowerGoodPort0    = 1;

    // Initialize event queue
    #if defined( USB_ENABLE_TRANSFER_EVENT )
        StructQueueInit(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
    #endif

    return TRUE;
}

/****************************************************************************
  Function:
    BYTE USBHostRead( BYTE deviceAddress, BYTE endpoint, BYTE *pData,
                        DWORD size )
  Description:
    This function initiates a read from the attached device.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device address
    BYTE endpoint       - Endpoint number
    BYTE *pData         - Pointer to where to store the data
    DWORD size          - Number of data bytes to store

  Return Values:
    USB_SUCCESS                     - Read started successfully.
    USB_UNKNOWN_DEVICE              - Device with the specified address not found.
    USB_INVALID_STATE               - We are not in a normal running state.
    USB_ENDPOINT_ILLEGAL_TYPE       - Must use USBHostControlRead to read
                                        from a control endpoint.
    USB_ENDPOINT_ILLEGAL_DIRECTION  - Must read from an IN endpoint.
    USB_ENDPOINT_STALLED            - Endpoint is stalled.  Must be cleared
                                        by the application.
    USB_ENDPOINT_ERROR              - Endpoint has too many errors.  Must be
                                        cleared by the application.
    USB_ENDPOINT_BUSY               - A Read is already in progress.
    USB_ENDPOINT_NOT_FOUND          - Invalid endpoint.

  Remarks:
    None
  ***************************************************************************/

BYTE USBHostRead( BYTE deviceAddress, BYTE endpoint, BYTE *pData, DWORD size )
{
    USB_ENDPOINT_INFO *ep;

    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    // If we are not in a normal user running state, we cannot do this.
    if ((usbHostState & STATE_MASK) != STATE_RUNNING)
    {
        return USB_INVALID_STATE;
    }

    ep = pEndpointList;
    while (ep != NULL)
    {
        if (ep->bEndpointAddress == endpoint)
        {
            if (ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_CONTROL)
            {
                // Must not be a control endpoint.
                return USB_ENDPOINT_ILLEGAL_TYPE;
            }

            if (!(ep->bEndpointAddress & 0x80))
            {
                // Trying to do an IN with an OUT endpoint.
                return USB_ENDPOINT_ILLEGAL_DIRECTION;
            }

            if (ep->status.bfStalled)
            {
                // The endpoint is stalled.  It must be restarted before a write
                // can be performed.
                return USB_ENDPOINT_STALLED;
            }

            if (ep->status.bfError)
            {
                // The endpoint has errored.  The error must be cleared before a
                // write can be performed.
                return USB_ENDPOINT_ERROR;
            }

            if (!ep->status.bfTransferComplete)
            {
                // We are already processing a request for this endpoint.
                return USB_ENDPOINT_BUSY;
            }

            _USB_InitRead( ep, pData, size );

            return USB_SUCCESS;
        }
        ep = ep->next;
    }
    return USB_ENDPOINT_NOT_FOUND;   // Endpoint not found
}

/****************************************************************************
  Function:
    BYTE USBHostResetDevice( BYTE deviceAddress )

  Summary:
    This function resets an attached device.

  Description:
    This function places the device back in the RESET state, to issue RESET
    signaling.  It can be called only if the state machine is not in the
    DETACHED state.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device address

  Return Values:
    USB_SUCCESS         - Success
    USB_UNKNOWN_DEVICE  - Device not found
    USB_ILLEGAL_REQUEST - Device cannot RESUME unless it is suspended

  Remarks:
    In order to do a full clean-up, the state is set back to STATE_DETACHED
    rather than a reset state.  The ATTACH interrupt will automatically be
    triggered when the module is re-enabled, and the proper reset will be
    performed.
  ***************************************************************************/

BYTE USBHostResetDevice( BYTE deviceAddress )
{
    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    if ((usbHostState & STATE_MASK) == STATE_DETACHED)
    {
        return USB_ILLEGAL_REQUEST;
    }

    usbHostState = STATE_DETACHED;

    return USB_SUCCESS;
}

/****************************************************************************
  Function:
    BYTE USBHostResumeDevice( BYTE deviceAddress )

  Summary:
    This function issues a RESUME to the attached device.

  Description:
    This function issues a RESUME to the attached device.  It can called only
    if the state machine is in the suspend state.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device address

  Return Values:
    USB_SUCCESS         - Success
    USB_UNKNOWN_DEVICE  - Device not found
    USB_ILLEGAL_REQUEST - Device cannot RESUME unless it is suspended

  Remarks:
    None
  ***************************************************************************/

BYTE USBHostResumeDevice( BYTE deviceAddress )
{
    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    if (usbHostState != (STATE_RUNNING | SUBSTATE_SUSPEND_AND_RESUME | SUBSUBSTATE_SUSPEND))
    {
        return USB_ILLEGAL_REQUEST;
    }

    // Advance the state machine to issue resume signalling.
    _USB_SetNextSubSubState();

    return USB_SUCCESS;
}

/****************************************************************************
  Function:
    BYTE USBHostSetDeviceConfiguration( BYTE deviceAddress,
                    BYTE configuration )

  Summary:
    This function changes the device's configuration.

  Description:
    This function is used by the application to change the device's
    Configuration.  This function must be used instead of
    USBHostDeviceRequest(), because the endpoint definitions may change.

  Precondition:
    The host state machine should be in the running state, and no reads or
    writes should be in progress.

  Parameters:
    BYTE deviceAddress  - Device address
    BYTE configuration  - Index of the new configuration

  Return Values:
    USB_SUCCESS         - Process of changing the configuration was started
                            successfully.
    USB_UNKNOWN_DEVICE  - Device not found
    USB_INVALID_STATE   - This function cannot be called during enumeration
                            or while performing a device request.
    USB_BUSY            - No IN or OUT transfers may be in progress.

  Remarks:
    If an invalid configuration is specified, this function cannot return
    an error.  Instead, the event USB_UNSUPPORTED_DEVICE will the sent to the
    application layer and the device will be placed in a holding state with a
    USB_HOLDING_UNSUPPORTED_DEVICE error.
  ***************************************************************************/

BYTE USBHostSetDeviceConfiguration( BYTE deviceAddress, BYTE configuration )
{
    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    // If we are not in a normal user running state, we cannot do this.
    if ((usbHostState & STATE_MASK) != STATE_RUNNING)
    {
        return USB_INVALID_STATE;
    }

    // Make sure no other reads or writes are in progress.
    if (_USB_TransferInProgress())
    {
        return USB_BUSY;
    }

    // Set the new device configuration.
    usbDeviceInfo.currentConfiguration = configuration;

    // Set the state back to configure the device.  This will destroy the
    // endpoint list and terminate any current transactions.  We already have
    // the configuration, so we can jump into the Select Configuration state.
    // If the configuration value is invalid, the state machine will error and
    // put the device into a holding state.
    usbHostState = STATE_CONFIGURING | SUBSTATE_SELECT_CONFIGURATION;

    return USB_SUCCESS;
}


/****************************************************************************
  Function:
    BYTE USBHostSetNAKTimeout( BYTE deviceAddress, BYTE endpoint, WORD flags,
                WORD timeoutCount )

  Summary:
    This function specifies NAK timeout capability.

  Description:
    This function is used to set whether or not an endpoint on a device
    should time out a transaction based on the number of NAKs received, and
    if so, how many NAKs are allowed before the timeout.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device address
    BYTE endpoint       - Endpoint number to configure
    WORD flags          - Bit 0:
                            * 0 = disable NAK timeout
                            * 1 = enable NAK timeout
    WORD timeoutCount   - Number of NAKs allowed before a timeout

  Return Values:
    USB_SUCCESS             - NAK timeout was configured successfully.
    USB_UNKNOWN_DEVICE      - Device not found.
    USB_ENDPOINT_NOT_FOUND  - The specified endpoint was not found.

  Remarks:
    None
  ***************************************************************************/

BYTE USBHostSetNAKTimeout( BYTE deviceAddress, BYTE endpoint, WORD flags, WORD timeoutCount )
{
    USB_ENDPOINT_INFO *ep;

    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    ep = pEndpointList;
    while (ep != NULL)
    {
        if (ep->bEndpointAddress == endpoint)
        {
            ep->status.bfNAKTimeoutEnabled  = flags & 0x01;
            ep->timeoutNAKs                 = timeoutCount;

            return USB_SUCCESS;
        }
        ep = ep->next;
    }
    return USB_ENDPOINT_NOT_FOUND;
}


/****************************************************************************
  Function:
    void USBHostShutdown( void )

  Description:
    This function turns off the USB module and frees all unnecessary memory.
    This routine can be called by the application layer to shut down all
    USB activity, which effectively detaches all devices.  The event
    EVENT_DETACH will be sent to the client drivers for the attached device,
    and the event EVENT_VBUS_RELEASE_POWER will be sent to the application
    layer.

  Precondition:
    None

  Parameters:
    None - None

  Returns:
    None

  Remarks:
    None
  ***************************************************************************/

void USBHostShutdown( void )
{
    // Shut off the power to the module first, in case we are in an
    // overcurrent situation.

    #ifdef  USB_SUPPORT_OTG
        if (!USBOTGHnpIsActive())
        {
            // If we currently have an attached device, notify the higher layers that
            // the device is being removed.
            if (usbDeviceInfo.deviceAddress)
            {
                USB_VBUS_POWER_EVENT_DATA   powerRequest;

                powerRequest.port = 0;  // Currently was have only one port.

                USB_HOST_APP_EVENT_HANDLER( usbDeviceInfo.deviceAddress, EVENT_VBUS_RELEASE_POWER,
                    &powerRequest, sizeof(USB_VBUS_POWER_EVENT_DATA) );
                _USB_NotifyClients(usbDeviceInfo.deviceAddress, EVENT_DETACH,
                    &usbDeviceInfo.deviceAddress, sizeof(BYTE) );


            }
        }
    #else
        U1PWRC = USB_NORMAL_OPERATION | USB_DISABLED;  //MR - Turning off Module will cause unwanted Suspends in OTG

        // If we currently have an attached device, notify the higher layers that
        // the device is being removed.
        if (usbDeviceInfo.deviceAddress)
        {
            USB_VBUS_POWER_EVENT_DATA   powerRequest;

            powerRequest.port = 0;  // Currently was have only one port.

            USB_HOST_APP_EVENT_HANDLER( usbDeviceInfo.deviceAddress, EVENT_VBUS_RELEASE_POWER,
                &powerRequest, sizeof(USB_VBUS_POWER_EVENT_DATA) );
            _USB_NotifyClients(usbDeviceInfo.deviceAddress, EVENT_DETACH,
                &usbDeviceInfo.deviceAddress, sizeof(BYTE) );


        }
    #endif

    // Free all extra allocated memory, initialize variables, and reset the
    // state machine.
    USBHostInit( 0 );
}


/****************************************************************************
  Function:
    BYTE USBHostSuspendDevice( BYTE deviceAddress )

  Summary:
    This function suspends a device.

  Description:
    This function put a device into an IDLE state.  It can only be called
    while the state machine is in normal running mode.  After 3ms, the
    attached device should go into SUSPEND mode.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device to suspend

  Return Values:
    USB_SUCCESS         - Success
    USB_UNKNOWN_DEVICE  - Device not found
    USB_ILLEGAL_REQUEST - Cannot suspend unless device is in normal run mode

  Remarks:
    None
  ***************************************************************************/

BYTE USBHostSuspendDevice( BYTE deviceAddress )
{
    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    if (usbHostState != (STATE_RUNNING | SUBSTATE_NORMAL_RUN))
    {
        return USB_ILLEGAL_REQUEST;
    }

    // Turn off SOF's, so the bus is idle.
    U1CONbits.SOFEN = 0;

    // Put the state machine in suspend mode.
    usbHostState = STATE_RUNNING | SUBSTATE_SUSPEND_AND_RESUME | SUBSUBSTATE_SUSPEND;

    return USB_SUCCESS;
}

/****************************************************************************
  Function:
    void USBHostTasks( void )

  Summary:
    This function executes the host tasks for USB host operation.

  Description:
    This function executes the host tasks for USB host operation.  It must be
    executed on a regular basis to keep everything functioning.

    The primary purpose of this function is to handle device attach/detach
    and enumeration.  It does not handle USB packet transmission or
    reception; that must be done in the USB interrupt handler to ensure
    timely operation.

    This routine should be called on a regular basis, but there is no
    specific time requirement.  Devices will still be able to attach,
    enumerate, and detach, but the operations will occur more slowly as the
    calling interval increases.

  Precondition:
    USBHostInit() has been called.

  Parameters:
    None

  Returns:
    None

  Remarks:
    None
  ***************************************************************************/

void USBHostTasks( void )
{
    static USB_CONFIGURATION           *pCurrentConfigurationNode;  //MR - made static for OTG
    USB_INTERFACE_INFO          *pCurrentInterface;
    BYTE                        *pTemp;
    BYTE                        temp;
    USB_VBUS_POWER_EVENT_DATA   powerRequest;

    #ifdef DEBUG_MODE
//        UART2PutChar('<');
//        UART2PutHex( usbHostState>>8 );
//        UART2PutHex( usbHostState & 0xff );
//        UART2PutChar('-');
//        UART2PutHex( pCurrentEndpoint->transferState );
//        UART2PutChar('>');
    #endif

    // The PIC32MX detach interrupt is not reliable.  If we are not in one of
    // the detached states, we'll do a check here to see if we've detached.
    // If the ATTACH bit is 0, we have detached.
    #ifdef __PIC32MX__
        #ifdef USE_MANUAL_DETACH_DETECT
            if (((usbHostState & STATE_MASK) != STATE_DETACHED) && !U1IRbits.ATTACHIF)
            {
                #ifdef DEBUG_MODE
                    UART2PutChar( '>' );
                    UART2PutChar( ']' );
                #endif
                usbHostState = STATE_DETACHED;
            }
        #endif
    #endif

    // Send any queued events to the client and application layers.
    #if defined ( USB_ENABLE_TRANSFER_EVENT )
    {
        USB_EVENT_DATA *item;
        #if defined( __C30__ )
            WORD        interrupt_mask;
        #elif defined( __PIC32MX__ )
            UINT32      interrupt_mask;
        #else
            #error Cannot save interrupt status
        #endif

        while (StructQueueIsNotEmpty(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
        {
            item = StructQueuePeekTail(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);

            switch(item->event)
            {
                case EVENT_TRANSFER:
                case EVENT_BUS_ERROR:
                    _USB_NotifyClients( usbDeviceInfo.deviceAddress, item->event, &item->TransferData, sizeof(HOST_TRANSFER_DATA) );
                    break;
                default:
                    break;
            }

            // Guard against USB interrupts
            interrupt_mask = U1IE;
            U1IE = 0;

            StructQueueRemove(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);

            // Re-enable USB interrupts
            U1IE = interrupt_mask;
        }
    }
    #endif

    // See if we got an interrupt to change our state.
    if (usbOverrideHostState != NO_STATE)
    {
        #ifdef DEBUG_MODE
            UART2PutChar('>');
        #endif
        usbHostState = usbOverrideHostState;
        usbOverrideHostState = NO_STATE;
    }

    //-------------------------------------------------------------------------
    // Main State Machine

    switch (usbHostState & STATE_MASK)
    {
        case STATE_DETACHED:
            switch (usbHostState & SUBSTATE_MASK)
            {
                case SUBSTATE_INITIALIZE:
                    // We got here either from initialization or from the user
                    // unplugging the device at any point in time.

                    // Turn off the module and free up memory.
                    USBHostShutdown();

                    #ifdef DEBUG_MODE
                        UART2PrintString( "HOST: Initializing DETACHED state.\r\n" );
                    #endif

                    // Initialize Endpoint 0 attributes.
                    pEndpointList->next                         = NULL;
                    pEndpointList->status.val                   = 0x00;
                    pEndpointList->status.bfUseDTS              = 1;
                    pEndpointList->status.bfTransferComplete    = 1;    // Initialize to success to allow preprocessing loops.
                    pEndpointList->status.bfNAKTimeoutEnabled   = 1;    // So we can catch devices that NAK forever during enumeration
                    pEndpointList->timeoutNAKs                  = USB_NUM_CONTROL_NAKS;
                    pEndpointList->wMaxPacketSize               = 64;
                    pEndpointList->dataCount                    = 0;    // Initialize to 0 since we set bfTransferComplete.
                    pEndpointList->bEndpointAddress             = 0;
                    pEndpointList->transferState                = TSTATE_IDLE;
                    pEndpointList->bmAttributes.bfTransferType  = USB_TRANSFER_TYPE_CONTROL;
                    pEndpointList->pInterface                   = NULL;

                    // Initialize any device specific information.
                    numEnumerationTries                 = USB_NUM_ENUMERATION_TRIES;
                    usbDeviceInfo.currentConfiguration  = 0; // Will be overwritten by config process or the user later
                    usbDeviceInfo.attributesOTG         = 0;
                    usbDeviceInfo.deviceAddressAndSpeed = 0;
                    usbDeviceInfo.flags.val             = 0;

                    // Set up the hardware.
                    U1IE                = 0;        // Clear and turn off interrupts.
                    U1IR                = 0xFF;
                    U1OTGIE             &= 0x8C;
                    U1OTGIR             = 0x7D;
                    U1EIE               = 0;
                    U1EIR               = 0xFF;

                    // Initialize the Buffer Descriptor Table pointer.
                    #if defined(__C30__)
                       U1BDTP1 = (WORD)(&BDT) >> 8;
                    #elif defined(__PIC32MX__)
                       U1BDTP1 = ((DWORD)KVA_TO_PA(&BDT) & 0x0000FF00) >> 8;
                       U1BDTP2 = ((DWORD)KVA_TO_PA(&BDT) & 0x00FF0000) >> 16;
                       U1BDTP3 = ((DWORD)KVA_TO_PA(&BDT) & 0xFF000000) >> 24;
                    #else
                        #error Cannot set up the Buffer Descriptor Table pointer.
                    #endif

                    // Configure the module
                    U1CON               = USB_HOST_MODE_ENABLE | USB_SOF_DISABLE;                       // Turn of SOF's to cut down noise
                    U1CON               = USB_HOST_MODE_ENABLE | USB_PINGPONG_RESET | USB_SOF_DISABLE;  // Reset the ping-pong buffers
                    U1CON               = USB_HOST_MODE_ENABLE | USB_SOF_DISABLE;                       // Release the ping-pong buffers
                    #ifdef  USB_SUPPORT_OTG
                        U1OTGCON            |= USB_DPLUS_PULLDOWN_ENABLE | USB_DMINUS_PULLDOWN_ENABLE | USB_OTG_ENABLE; // Pull down D+ and D-
                    #else
                        U1OTGCON            = USB_DPLUS_PULLDOWN_ENABLE | USB_DMINUS_PULLDOWN_ENABLE; // Pull down D+ and D-
                    #endif

                    #if defined(__PIC32MX__)
                        U1OTGCON |= USB_VBUS_ON;
                    #endif

                    U1CNFG1             = USB_STOP_IN_IDLE_MODE | USB_PING_PONG_MODE;
                    #if defined(__C30__)
                        U1CNFG2         = USB_VBUS_BOOST_ENABLE | USB_VBUS_COMPARE_ENABLE | USB_ONCHIP_ENABLE;
                    #endif
                    U1ADDR              = 0;                        // Set default address and LSPDEN to 0
                    U1EP0bits.LSPD      = 0;
                    U1SOF               = USB_SOF_THRESHOLD_64;     // Maximum EP0 packet size

                    // Set the next substate.  We do this before we enable
                    // interrupts in case the interrupt changes states.
                    _USB_SetNextSubState();
                    break;

                case SUBSTATE_WAIT_FOR_POWER:
                    // We will wait here until the application tells us we can
                    // turn on power.
                    if (usbRootHubInfo.flags.bPowerGoodPort0)
                    {
                        _USB_SetNextSubState();
                    }
                    break;

                case SUBSTATE_TURN_ON_POWER:
                    powerRequest.port       = 0;
                    powerRequest.current    = USB_INITIAL_VBUS_CURRENT;
                    if (USB_HOST_APP_EVENT_HANDLER( USB_ROOT_HUB, EVENT_VBUS_REQUEST_POWER,
                            &powerRequest, sizeof(USB_VBUS_POWER_EVENT_DATA) ))
                    {
                        // Power on the module
                        U1PWRC                = USB_NORMAL_OPERATION | USB_ENABLED;

                        #if defined( __C30__ )
                            IFS5            &= 0xFFBF;
                            IPC21           &= 0xF0FF;
                            IPC21           |= 0x0600;
                            IEC5            |= 0x0040;
                        #elif defined( __PIC32MX__ )
                            // Enable the USB interrupt.
                            IFS1CLR         = 0x02000000;
                            IPC11CLR        = 0x0000FF00;
                            IPC11SET        = 0x00001000;
                            IEC1SET         = 0x02000000;
                        #else
                            #error Cannot enable USB interrupt.
                        #endif

                        // Enable the ATTACH interrupt.
                        U1IEbits.ATTACHIE = 1;

                        // Set the next substate.  We do this before we enable
                        // interrupts in case the interrupt changes states.
                        _USB_SetNextSubState();
                    }
                    else
                    {
                        usbRootHubInfo.flags.bPowerGoodPort0 = 0;
                        usbHostState = STATE_DETACHED | SUBSTATE_WAIT_FOR_POWER;
                    }
                    break;

                case SUBSTATE_WAIT_FOR_DEVICE:
                    // Wait here for the ATTACH interrupt.
                    #ifdef  USB_SUPPORT_OTG
                        U1IEbits.ATTACHIE = 1;
                    #endif
                break;
            }
            break;

        case STATE_ATTACHED:
            switch (usbHostState & SUBSTATE_MASK)
            {
                case SUBSTATE_SETTLE:
                    // Wait 100ms for the insertion process to complete and power
                    // at the device to be stable.
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_START_SETTLING_DELAY:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Starting settling delay.\r\n" );
                            #endif

                            // Clear and turn on the DETACH interrupt.
                            U1IR                    = USB_INTERRUPT_DETACH;   // The interrupt is cleared by writing a '1' to the flag.
                            U1IEbits.DETACHIE       = 1;

                            // Configure and turn on the settling timer - 100ms.
                            numTimerInterrupts      = USB_INSERT_TIME;
                            U1OTGIR                 = USB_INTERRUPT_T1MSECIF; // The interrupt is cleared by writing a '1' to the flag.
                            U1OTGIEbits.T1MSECIE    = 1;
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_WAIT_FOR_SETTLING:
                            // Wait for the timer to finish in the background.
                            break;

                        case SUBSUBSTATE_SETTLING_DONE:
                            _USB_SetNextSubState();
                            break;

                        default:
                            // We shouldn't get here.
                            break;
                    }
                    break;

                case SUBSTATE_RESET_DEVICE:
                    // Reset the device.  We have to do the reset timing ourselves.
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SET_RESET:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Resetting the device.\r\n" );
                            #endif

                            // Prepare a data buffer for us to use.  We'll make it 8 bytes for now,
                            // which is the minimum wMaxPacketSize for EP0.
                            if (pEP0Data != NULL)
                            {
                                freez( pEP0Data );
                            }
                            if ((pEP0Data = (BYTE *)malloc( 8 )) == NULL)
                            {
                                #ifdef DEBUG_MODE
                                    UART2PrintString( "HOST: Error alloc-ing pEP0Data\r\n" );
                                #endif
                                _USB_SetErrorCode( USB_HOLDING_OUT_OF_MEMORY );
                                _USB_SetHoldState();
                                break;
                            }

                            // Initialize the USB Device information
                            usbDeviceInfo.currentConfiguration      = 0;
                            usbDeviceInfo.attributesOTG             = 0;
                            usbDeviceInfo.flags.val                 = 0;

                            _USB_InitErrorCounters();

                            // Disable all EP's except EP0.
                            U1EP0  = USB_ENDPOINT_CONTROL_SETUP;
                            U1EP1  = USB_DISABLE_ENDPOINT;
                            U1EP2  = USB_DISABLE_ENDPOINT;
                            U1EP3  = USB_DISABLE_ENDPOINT;
                            U1EP4  = USB_DISABLE_ENDPOINT;
                            U1EP5  = USB_DISABLE_ENDPOINT;
                            U1EP6  = USB_DISABLE_ENDPOINT;
                            U1EP7  = USB_DISABLE_ENDPOINT;
                            U1EP8  = USB_DISABLE_ENDPOINT;
                            U1EP9  = USB_DISABLE_ENDPOINT;
                            U1EP10 = USB_DISABLE_ENDPOINT;
                            U1EP11 = USB_DISABLE_ENDPOINT;
                            U1EP12 = USB_DISABLE_ENDPOINT;
                            U1EP13 = USB_DISABLE_ENDPOINT;
                            U1EP14 = USB_DISABLE_ENDPOINT;
                            U1EP15 = USB_DISABLE_ENDPOINT;

                            // See if the device is low speed.
                            if (!U1CONbits.JSTATE)
                            {
                                #ifdef DEBUG_MODE
                                    UART2PrintString( "HOST: Low Speed!\r\n" );
                                #endif
                                usbDeviceInfo.flags.bfIsLowSpeed    = 1;
                                usbDeviceInfo.deviceAddressAndSpeed = 0x80;
                                U1ADDR                              = 0x80;
                                U1EP0bits.LSPD                      = 1;
                            }

                            // Reset all ping-pong buffers if they are being used.
                            U1CONbits.PPBRST                    = 1;
                            U1CONbits.PPBRST                    = 0;
                            usbDeviceInfo.flags.bfPingPongIn    = 0;
                            usbDeviceInfo.flags.bfPingPongOut   = 0;

                            #ifdef  USB_SUPPORT_OTG
                                //Disable HNP
                                USBOTGDisableHnp();
                                USBOTGDeactivateHnp();
                            #endif

                            // Assert reset for 10ms.  Start a timer countdown.
                            U1CONbits.USBRST                    = 1;
                            numTimerInterrupts                  = USB_RESET_TIME;
                            //U1OTGIRbits.T1MSECIF                = 1;       // The interrupt is cleared by writing a '1' to the flag.
                            U1OTGIR                             = USB_INTERRUPT_T1MSECIF; // The interrupt is cleared by writing a '1' to the flag.
                            U1OTGIEbits.T1MSECIE                = 1;

                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_RESET_WAIT:
                            // Wait for the timer to finish in the background.
                            break;

                        case SUBSUBSTATE_RESET_RECOVERY:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Reset complete.\r\n" );
                            #endif

                            // Deassert reset.
                            U1CONbits.USBRST        = 0;

                            // Start sending SOF's.
                            U1CONbits.SOFEN         = 1;

                            // Wait for the reset recovery time.
                            numTimerInterrupts      = USB_RESET_RECOVERY_TIME;
                            U1OTGIR                 = USB_INTERRUPT_T1MSECIF; // The interrupt is cleared by writing a '1' to the flag.
                            U1OTGIEbits.T1MSECIE    = 1;

                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_RECOVERY_WAIT:
                            // Wait for the timer to finish in the background.
                            break;

                        case SUBSUBSTATE_RESET_COMPLETE:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Reset complete.\r\n" );
                            #endif

                            // Enable USB interrupts
                            U1IE                    = USB_INTERRUPT_TRANSFER | USB_INTERRUPT_SOF | USB_INTERRUPT_ERROR | USB_INTERRUPT_DETACH;
                            U1EIE                   = 0xFF;

                            _USB_SetNextSubState();
                            break;

                        default:
                            // We shouldn't get here.
                            break;
                    }
                    break;

                case SUBSTATE_GET_DEVICE_DESCRIPTOR_SIZE:
                    // Send the GET DEVICE DESCRIPTOR command to get just the size
                    // of the descriptor and the max packet size, so we can allocate
                    // a large enough buffer for getting the whole thing and enough
                    // buffer space for each piece.
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SEND_GET_DEVICE_DESCRIPTOR_SIZE:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Getting Device Descriptor size.\r\n" );
                            #endif

                            // Set up and send GET DEVICE DESCRIPTOR
                            if (pDeviceDescriptor != NULL)
                            {
                                freez( pDeviceDescriptor );
                            }

                            pEP0Data[0] = USB_SETUP_DEVICE_TO_HOST | USB_SETUP_TYPE_STANDARD | USB_SETUP_RECIPIENT_DEVICE;
                            pEP0Data[1] = USB_REQUEST_GET_DESCRIPTOR;
                            pEP0Data[2] = 0; // Index
                            pEP0Data[3] = USB_DESCRIPTOR_DEVICE; // Type
                            pEP0Data[4] = 0;
                            pEP0Data[5] = 0;
                            pEP0Data[6] = 8;
                            pEP0Data[7] = 0;

                            _USB_InitControlRead( pEndpointList, pEP0Data, 8, pEP0Data, 8 );
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_WAIT_FOR_GET_DEVICE_DESCRIPTOR_SIZE:
                            if (pEndpointList->status.bfTransferComplete)
                            {
                                if (pEndpointList->status.bfTransferSuccessful)
                                {
                                    #ifndef USB_HUB_SUPPORT_INCLUDED
                                        // See if a hub is attached.  Hubs are not supported.
                                        if (pEP0Data[4] == USB_HUB_CLASSCODE)   // bDeviceClass
                                        {
                                            _USB_SetErrorCode( USB_HOLDING_UNSUPPORTED_HUB );
                                            _USB_SetHoldState();
                                        }
                                        else
                                        {
                                            _USB_SetNextSubSubState();
                                        }
                                    #else
                                        _USB_SetNextSubSubState();
                                    #endif
                                }
                                else
                                {
                                    // We are here because of either a STALL or a NAK.  See if
                                    // we have retries left to try the command again or try to
                                    // enumerate again.
                                    _USB_CheckCommandAndEnumerationAttempts();
                                }
                            }
                            break;

                        case SUBSUBSTATE_GET_DEVICE_DESCRIPTOR_SIZE_COMPLETE:
                            // Allocate a buffer for the entire Device Descriptor
                            if ((pDeviceDescriptor = (BYTE *)malloc( *pEP0Data )) == NULL)
                            {
                                // We cannot continue.  Freeze until the device is removed.
                                _USB_SetErrorCode( USB_HOLDING_OUT_OF_MEMORY );
                                _USB_SetHoldState();
                                break;
                            }
                            // Save the descriptor size in the descriptor (bLength)
                            *pDeviceDescriptor = *pEP0Data;

                            // Set the EP0 packet size.
                            pEndpointList->wMaxPacketSize = ((USB_DEVICE_DESCRIPTOR *)pEP0Data)->bMaxPacketSize0;

                            // Make our pEP0Data buffer the size of the max packet.
                            freez( pEP0Data );
                            if ((pEP0Data = (BYTE *)malloc( pEndpointList->wMaxPacketSize )) == NULL)
                            {
                                // We cannot continue.  Freeze until the device is removed.
                                #ifdef DEBUG_MODE
                                    UART2PrintString( "HOST: Error re-alloc-ing pEP0Data\r\n" );
                                #endif
                                _USB_SetErrorCode( USB_HOLDING_OUT_OF_MEMORY );
                                _USB_SetHoldState();
                                break;
                            }

                            // Clean up and advance to the next substate.
                            _USB_InitErrorCounters();
                            _USB_SetNextSubState();
                            break;

                        default:
                            break;
                    }
                    break;

                case SUBSTATE_GET_DEVICE_DESCRIPTOR:
                    // Send the GET DEVICE DESCRIPTOR command and receive the response
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SEND_GET_DEVICE_DESCRIPTOR:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Getting device descriptor.\r\n" );
                            #endif

                            // If we are currently sending a token, we cannot do anything.
                            if (usbBusInfo.flags.bfTokenAlreadyWritten)   //(U1CONbits.TOKBUSY)
                                break;

                            // Set up and send GET DEVICE DESCRIPTOR
                            pEP0Data[0] = USB_SETUP_DEVICE_TO_HOST | USB_SETUP_TYPE_STANDARD | USB_SETUP_RECIPIENT_DEVICE;
                            pEP0Data[1] = USB_REQUEST_GET_DESCRIPTOR;
                            pEP0Data[2] = 0; // Index
                            pEP0Data[3] = USB_DESCRIPTOR_DEVICE; // Type
                            pEP0Data[4] = 0;
                            pEP0Data[5] = 0;
                            pEP0Data[6] = *pDeviceDescriptor;
                            pEP0Data[7] = 0;
                            _USB_InitControlRead( pEndpointList, pEP0Data, 8, pDeviceDescriptor, *pDeviceDescriptor  );
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_WAIT_FOR_GET_DEVICE_DESCRIPTOR:
                            if (pEndpointList->status.bfTransferComplete)
                            {
                                if (pEndpointList->status.bfTransferSuccessful)
                                {
                                    _USB_SetNextSubSubState();
                                }
                                else
                                {
                                    // We are here because of either a STALL or a NAK.  See if
                                    // we have retries left to try the command again or try to
                                    // enumerate again.
                                    _USB_CheckCommandAndEnumerationAttempts();
                                }
                            }
                            break;

                        case SUBSUBSTATE_GET_DEVICE_DESCRIPTOR_COMPLETE:
                            // Clean up and advance to the next substate.
                            _USB_InitErrorCounters();
                            _USB_SetNextSubState();
                            break;

                        default:
                            break;
                    }
                    break;

                case SUBSTATE_VALIDATE_VID_PID:
                    #ifdef DEBUG_MODE
                        UART2PrintString( "HOST: Validating VID and PID.\r\n" );
                    #endif

                    // Search the TPL for the device's VID & PID.  If a client driver is
                    // available for the over-all device, use it.  Otherwise, we'll search
                    // again later for an appropriate class driver.
                    _USB_FindDeviceLevelClientDriver();

                    // Advance to the next state to assign an address to the device.
                    //
                    // Note: We assign an address to all devices and hold later if
                    // we can't find a supported configuration.
                    _USB_SetNextState();
                    break;
            }
            break;

        case STATE_ADDRESSING:
            switch (usbHostState & SUBSTATE_MASK)
            {
                case SUBSTATE_SET_DEVICE_ADDRESS:
                    // Send the SET ADDRESS command.  We can't set the device address
                    // in hardware until the entire transaction is complete.
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SEND_SET_DEVICE_ADDRESS:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Setting device address.\r\n" );
                            #endif

                            // Select an address for the device.  Store it so we can access it again
                            // easily.  We'll put the low speed indicator on later.
                            // This has been broken out so when we allow multiple devices, we have
                            // a single interface point to allocate a new address.
                            usbDeviceInfo.deviceAddress = USB_SINGLE_DEVICE_ADDRESS;

                            // Set up and send SET ADDRESS
                            pEP0Data[0] = USB_SETUP_HOST_TO_DEVICE | USB_SETUP_TYPE_STANDARD | USB_SETUP_RECIPIENT_DEVICE;
                            pEP0Data[1] = USB_REQUEST_SET_ADDRESS;
                            pEP0Data[2] = usbDeviceInfo.deviceAddress;
                            pEP0Data[3] = 0;
                            pEP0Data[4] = 0;
                            pEP0Data[5] = 0;
                            pEP0Data[6] = 0;
                            pEP0Data[7] = 0;
                            _USB_InitControlWrite( pEndpointList, pEP0Data, 8, NULL, 0 );
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_WAIT_FOR_SET_DEVICE_ADDRESS:
                            if (pEndpointList->status.bfTransferComplete)
                            {
                                if (pEndpointList->status.bfTransferSuccessful)
                                {
                                    _USB_SetNextSubSubState();
                                }
                                else
                                {
                                    // We are here because of either a STALL or a NAK.  See if
                                    // we have retries left to try the command again or try to
                                    // enumerate again.
                                    _USB_CheckCommandAndEnumerationAttempts();
                                }
                            }
                            break;

                        case SUBSUBSTATE_SET_DEVICE_ADDRESS_COMPLETE:
                            // Set the device's address here.
                            usbDeviceInfo.deviceAddressAndSpeed = (usbDeviceInfo.flags.bfIsLowSpeed << 7) | usbDeviceInfo.deviceAddress;

                            // Clean up and advance to the next state.
                            _USB_InitErrorCounters();
                            _USB_SetNextState();
                            break;

                        default:
                            break;
                    }
                    break;
            }
            break;

        case STATE_CONFIGURING:
            switch (usbHostState & SUBSTATE_MASK)
            {
                case SUBSTATE_INIT_CONFIGURATION:
                    // Delete the old list of configuration descriptors and
                    // initialize the counter.  We will request the descriptors
                    // from highest to lowest so the lowest will be first in
                    // the list.
                    countConfigurations = ((USB_DEVICE_DESCRIPTOR *)pDeviceDescriptor)->bNumConfigurations;
                    while (pConfigurationDescriptorList != NULL)
                    {
                        pTemp = (BYTE *)pConfigurationDescriptorList->next;
                        free( pConfigurationDescriptorList->descriptor );
                        free( pConfigurationDescriptorList );
                        pConfigurationDescriptorList = (USB_CONFIGURATION *)pTemp;
                    }
                    _USB_SetNextSubState();
                    break;

                case SUBSTATE_GET_CONFIG_DESCRIPTOR_SIZE:
                    // Get the size of the Configuration Descriptor for the current configuration
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SEND_GET_CONFIG_DESCRIPTOR_SIZE:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Getting Config Descriptor size.\r\n" );
                            #endif

                            // Set up and send GET CONFIGURATION (n) DESCRIPTOR with a length of 8
                            pEP0Data[0] = USB_SETUP_DEVICE_TO_HOST | USB_SETUP_TYPE_STANDARD | USB_SETUP_RECIPIENT_DEVICE;
                            pEP0Data[1] = USB_REQUEST_GET_DESCRIPTOR;
                            pEP0Data[2] = countConfigurations-1;    // USB 2.0 - range is 0 - count-1
                            pEP0Data[3] = USB_DESCRIPTOR_CONFIGURATION;
                            pEP0Data[4] = 0;
                            pEP0Data[5] = 0;
                            pEP0Data[6] = 8;
                            pEP0Data[7] = 0;
                            _USB_InitControlRead( pEndpointList, pEP0Data, 8, pEP0Data, 8 );
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_WAIT_FOR_GET_CONFIG_DESCRIPTOR_SIZE:
                            if (pEndpointList->status.bfTransferComplete)
                            {
                                if (pEndpointList->status.bfTransferSuccessful)
                                {
                                    _USB_SetNextSubSubState();
                                }
                                else
                                {
                                    // We are here because of either a STALL or a NAK.  See if
                                    // we have retries left to try the command again or try to
                                    // enumerate again.
                                    _USB_CheckCommandAndEnumerationAttempts();
                                }
                            }
                            break;

                        case SUBSUBSTATE_GET_CONFIG_DESCRIPTOR_SIZECOMPLETE:
                            // Allocate a buffer for an entry in the configuration descriptor list.
                            if ((pTemp = (BYTE *)malloc( sizeof (USB_CONFIGURATION) )) == NULL)
                            {
                                // We cannot continue.  Freeze until the device is removed.
                                _USB_SetErrorCode( USB_HOLDING_OUT_OF_MEMORY );
                                _USB_SetHoldState();
                                break;
                            }

                            // Allocate a buffer for the entire Configuration Descriptor
                            if ((((USB_CONFIGURATION *)pTemp)->descriptor = (BYTE *)malloc( ((WORD)pEP0Data[3] << 8) + (WORD)pEP0Data[2] )) == NULL)
                            {
                                // Not enough memory for the descriptor!
                                freez( pTemp );

                                // We cannot continue.  Freeze until the device is removed.
                                _USB_SetErrorCode( USB_HOLDING_OUT_OF_MEMORY );
                                _USB_SetHoldState();
                                break;
                            }

                            // Save wTotalLength
                            ((USB_CONFIGURATION_DESCRIPTOR *)((USB_CONFIGURATION *)pTemp)->descriptor)->wTotalLength =
                                    ((WORD)pEP0Data[3] << 8) + (WORD)pEP0Data[2];

                            // Put the new node at the front of the list.
                            ((USB_CONFIGURATION *)pTemp)->next = pConfigurationDescriptorList;
                            pConfigurationDescriptorList = (USB_CONFIGURATION *)pTemp;

                            // Save the configuration descriptor pointer and number
                            pCurrentConfigurationDescriptor            = ((USB_CONFIGURATION *)pTemp)->descriptor;
                            ((USB_CONFIGURATION *)pTemp)->configNumber = countConfigurations;

                            // Clean up and advance to the next state.
                            _USB_InitErrorCounters();
                            _USB_SetNextSubState();
                            break;

                        default:
                            break;
                    }
                    break;

                case SUBSTATE_GET_CONFIG_DESCRIPTOR:
                    // Get the entire Configuration Descriptor for this configuration
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SEND_GET_CONFIG_DESCRIPTOR:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Getting Config Descriptor.\r\n" );
                            #endif

                            // Set up and send GET CONFIGURATION (n) DESCRIPTOR.
                            pEP0Data[0] = USB_SETUP_DEVICE_TO_HOST | USB_SETUP_TYPE_STANDARD | USB_SETUP_RECIPIENT_DEVICE;
                            pEP0Data[1] = USB_REQUEST_GET_DESCRIPTOR;
                            pEP0Data[2] = countConfigurations-1;
                            pEP0Data[3] = USB_DESCRIPTOR_CONFIGURATION;
                            pEP0Data[4] = 0;
                            pEP0Data[5] = 0;
                            pEP0Data[6] = pConfigurationDescriptorList->descriptor[2];    // wTotalLength
                            pEP0Data[7] = pConfigurationDescriptorList->descriptor[3];
                            _USB_InitControlRead( pEndpointList, pEP0Data, 8, pConfigurationDescriptorList->descriptor,
                                    ((USB_CONFIGURATION_DESCRIPTOR *)pConfigurationDescriptorList->descriptor)->wTotalLength );
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_WAIT_FOR_GET_CONFIG_DESCRIPTOR:
                            if (pEndpointList->status.bfTransferComplete)
                            {
                                if (pEndpointList->status.bfTransferSuccessful)
                                {
                                    _USB_SetNextSubSubState();
                                }
                                else
                                {
                                    // We are here because of either a STALL or a NAK.  See if
                                    // we have retries left to try the command again or try to
                                    // enumerate again.
                                    _USB_CheckCommandAndEnumerationAttempts();
                                }
                            }
                            break;

                        case SUBSUBSTATE_GET_CONFIG_DESCRIPTOR_COMPLETE:
                            // Clean up and advance to the next state.  Keep the data for later use.
                            _USB_InitErrorCounters();
                            countConfigurations --;
                            if (countConfigurations)
                            {
                                // There are more descriptors that we need to get.
                                usbHostState = STATE_CONFIGURING | SUBSTATE_GET_CONFIG_DESCRIPTOR_SIZE;
                            }
                            else
                            {
                                // Start configuring the device.
                                _USB_SetNextSubState();
                              }
                            break;

                        default:
                            break;
                    }
                    break;

                case SUBSTATE_SELECT_CONFIGURATION:
                    // Set the OTG configuration of the device
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SELECT_CONFIGURATION:
                            // Free the old configuration (if any)
                            _USB_FreeConfigMemory();

                            // If the configuration wasn't selected based on the VID & PID
                            if (usbDeviceInfo.currentConfiguration == 0)
                            {
                                // Search for a supported class-specific configuration.
                                pCurrentConfigurationNode = pConfigurationDescriptorList;
                                while (pCurrentConfigurationNode)
                                {
                                    pCurrentConfigurationDescriptor = pCurrentConfigurationNode->descriptor;
                                    if (_USB_ParseConfigurationDescriptor())
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        // Free the memory allocated and
                                        // advance to  next configuration
                                        _USB_FreeConfigMemory();
                                        pCurrentConfigurationNode = pCurrentConfigurationNode->next;
                                    }
                                }
                            }
                            else
                            {
                                // Configuration selected by VID & PID, initialize data structures
                                pCurrentConfigurationNode = pConfigurationDescriptorList;
                                while (pCurrentConfigurationNode && pCurrentConfigurationNode->configNumber != usbDeviceInfo.currentConfiguration)
                                {
                                    pCurrentConfigurationNode = pCurrentConfigurationNode->next;
                                }
                                pCurrentConfigurationDescriptor = pCurrentConfigurationNode->descriptor;
                                if (!_USB_ParseConfigurationDescriptor())
                                {
                                    // Free the memory allocated, config attempt failed.
                                    _USB_FreeConfigMemory();
                                    pCurrentConfigurationNode = NULL;
                                }
                            }

                            //If No OTG Then
                            if (usbDeviceInfo.flags.bfConfiguredOTG)
                            {
                                // Did we fail to configure?
                                if (pCurrentConfigurationNode == NULL)
                                {
                                    // Failed to find a supported configuration.
                                    _USB_SetErrorCode( USB_HOLDING_UNSUPPORTED_DEVICE );
                                    _USB_SetHoldState();
                                }
                                else
                                {
                                    _USB_SetNextSubSubState();
                                }
                            }
                            else
                            {
                                _USB_SetNextSubSubState();
                            }
                            break;

                        case SUBSUBSTATE_SEND_SET_OTG:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Determine OTG capability.\r\n" );
                            #endif

                            // If the device does not support OTG, or
                            // if the device has already been configured, bail.
                            // Otherwise, send SET FEATURE to configure it.
                            if (!usbDeviceInfo.flags.bfConfiguredOTG)
                            {
                                #ifdef DEBUG_MODE
                                    UART2PrintString( "HOST: ...OTG needs configuring.\r\n" );
                                #endif
                                usbDeviceInfo.flags.bfConfiguredOTG = 1;

                                // Send SET FEATURE
                                pEP0Data[0] = USB_SETUP_HOST_TO_DEVICE | USB_SETUP_TYPE_STANDARD | USB_SETUP_RECIPIENT_DEVICE;
                                pEP0Data[1] = USB_REQUEST_SET_FEATURE;
                                if (usbDeviceInfo.flags.bfAllowHNP) // Needs to be set by the user
                                {
                                    pEP0Data[2] = OTG_FEATURE_B_HNP_ENABLE;
                                }
                                else
                                {
                                    pEP0Data[2] = OTG_FEATURE_A_HNP_SUPPORT;
                                }
                                pEP0Data[3] = 0;
                                pEP0Data[4] = 0;
                                pEP0Data[5] = 0;
                                pEP0Data[6] = 0;
                                pEP0Data[7] = 0;
                                _USB_InitControlWrite( pEndpointList, pEP0Data, 8, NULL, 0 );
                                _USB_SetNextSubSubState();
                            }
                            else
                            {
                                #ifdef DEBUG_MODE
                                    UART2PrintString( "HOST: ...No OTG.\r\n" );
                                #endif
                                _USB_SetNextSubState();
                            }
                            break;

                        case SUBSUBSTATE_WAIT_FOR_SET_OTG_DONE:
                            if (pEndpointList->status.bfTransferComplete)
                            {
                                if (pEndpointList->status.bfTransferSuccessful)
                                {
                                    #ifdef  USB_SUPPORT_OTG
                                        if (usbDeviceInfo.flags.bfAllowHNP)
                                        {
                                            USBOTGEnableHnp();
                                        }
                                     #endif
                                    _USB_SetNextSubSubState();
                                }
                                else
                                {
                                    #ifdef  USB_SUPPORT_OTG
                                        USBOTGDisableHnp();
                                    #endif
                                    // We are here because of either a STALL or a NAK.  See if
                                    // we have retries left to try the command again or try to
                                    // enumerate again.
                                    _USB_CheckCommandAndEnumerationAttempts();

                                    #if defined(DEBUG_MODE) && defined(USB_SUPPORT_OTG)
                                        UART2PrintString( "\r\n***** USB OTG Error - Set Feature B_HNP_ENABLE Stalled - Device Not Responding *****\r\n" );
                                    #endif
                                }
                            }
                            break;

                        case SUBSUBSTATE_SET_OTG_COMPLETE:
                             // Clean up and advance to the next state.
                           _USB_InitErrorCounters();

                            //MR - Moved For OTG Set Feature Support For Unsupported Devices
                            // Did we fail to configure?
                            if (pCurrentConfigurationNode == NULL)
                            {
                                // Failed to find a supported configuration.
                                _USB_SetErrorCode( USB_HOLDING_UNSUPPORTED_DEVICE );
                                _USB_SetHoldState();
                            }
                            else
                            {
                                //_USB_SetNextSubSubState();
                                _USB_InitErrorCounters();
                                _USB_SetNextSubState();
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case SUBSTATE_SET_CONFIGURATION:
                    // Set the configuration to the one specified for this device
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SEND_SET_CONFIGURATION:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Set configuration.\r\n" );
                            #endif

                            // Set up and send SET CONFIGURATION.
                            pEP0Data[0] = USB_SETUP_HOST_TO_DEVICE | USB_SETUP_TYPE_STANDARD | USB_SETUP_RECIPIENT_DEVICE;
                            pEP0Data[1] = USB_REQUEST_SET_CONFIGURATION;
                            pEP0Data[2] = usbDeviceInfo.currentConfiguration;
                            pEP0Data[3] = 0;
                            pEP0Data[4] = 0;
                            pEP0Data[5] = 0;
                            pEP0Data[6] = 0;
                            pEP0Data[7] = 0;
                            _USB_InitControlWrite( pEndpointList, pEP0Data, 8, NULL, 0 );
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_WAIT_FOR_SET_CONFIGURATION:
                            if (pEndpointList->status.bfTransferComplete)
                            {
                                if (pEndpointList->status.bfTransferSuccessful)
                                {
                                    _USB_SetNextSubSubState();
                                }
                                else
                                {
                                    // We are here because of either a STALL or a NAK.  See if
                                    // we have retries left to try the command again or try to
                                    // enumerate again.
                                    _USB_CheckCommandAndEnumerationAttempts();
                                }
                            }
                            break;

                        case SUBSUBSTATE_SET_CONFIGURATION_COMPLETE:
                            // Clean up and advance to the next state.
                            _USB_InitErrorCounters();
                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_INIT_CLIENT_DRIVERS:
                            #ifdef DEBUG_MODE
                                UART2PrintString( "HOST: Initializing client drivers...\r\n" );
                            #endif
                            _USB_SetNextState();
                            // Initialize client driver(s) for this configuration.
                            if (usbDeviceInfo.flags.bfUseDeviceClientDriver)
                            {
                                // We have a device that requires only one client driver.  Make sure
                                // that client driver can initialize this device.  If the client
                                // driver initialization fails, we cannot enumerate this device.
                                #ifdef DEBUG_MODE
                                    UART2PrintString( "HOST: Using device client driver.\r\n" );
                                #endif
                                temp = usbDeviceInfo.deviceClientDriver;
                                if (!usbClientDrvTable[temp].Initialize(usbDeviceInfo.deviceAddress, usbClientDrvTable[temp].flags))
                                {
                                    _USB_SetErrorCode( USB_HOLDING_CLIENT_INIT_ERROR );
                                    _USB_SetHoldState();
                                }
                            }
                            else
                            {
                                // We have a device that requires multiple client drivers.  Make sure
                                // every required client driver can initialize this device.  If any
                                // client driver initialization fails, we cannot enumerate the device.
                                #ifdef DEBUG_MODE
                                    UART2PrintString( "HOST: Scanning interfaces.\r\n" );
                                #endif
                                pCurrentInterface = pInterfaceList;
                                while (pCurrentInterface)
                                {
                                    temp = pCurrentInterface->clientDriver;
                                    if (!usbClientDrvTable[temp].Initialize(usbDeviceInfo.deviceAddress, usbClientDrvTable[temp].flags))
                                    {
                                        _USB_SetErrorCode( USB_HOLDING_CLIENT_INIT_ERROR );
                                        _USB_SetHoldState();
                                    }
                                    pCurrentInterface = pCurrentInterface->next;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                    break;
            }
            break;

        case STATE_RUNNING:
            switch (usbHostState & SUBSTATE_MASK)
            {
                case SUBSTATE_NORMAL_RUN:
                    break;

                case SUBSTATE_SUSPEND_AND_RESUME:
                    switch (usbHostState & SUBSUBSTATE_MASK)
                    {
                        case SUBSUBSTATE_SUSPEND:
                            // The IDLE state has already been set.  We need to wait here
                            // until the application decides to RESUME.
                            break;

                        case SUBSUBSTATE_RESUME:
                            // Issue a RESUME.
                            U1CONbits.RESUME = 1;

                            // Wait for the RESUME time.
                            numTimerInterrupts      = USB_RESUME_TIME;
                            U1OTGIR                 = USB_INTERRUPT_T1MSECIF; // The interrupt is cleared by writing a '1' to the flag.
                            U1OTGIEbits.T1MSECIE    = 1;

                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_RESUME_WAIT:
                            // Wait here until the timer expires.
                            break;

                        case SUBSUBSTATE_RESUME_RECOVERY:
                            // Turn off RESUME.
                            U1CONbits.RESUME        = 0;

                            // Start sending SOF's, so the device doesn't go back into the SUSPEND state.
                            U1CONbits.SOFEN         = 1;

                            // Wait for the RESUME recovery time.
                            numTimerInterrupts      = USB_RESUME_RECOVERY_TIME;
                            U1OTGIR                 = USB_INTERRUPT_T1MSECIF; // The interrupt is cleared by writing a '1' to the flag.
                            U1OTGIEbits.T1MSECIE    = 1;

                            _USB_SetNextSubSubState();
                            break;

                        case SUBSUBSTATE_RESUME_RECOVERY_WAIT:
                            // Wait here until the timer expires.
                            break;

                        case SUBSUBSTATE_RESUME_COMPLETE:
                            // Go back to normal running.
                            usbHostState = STATE_RUNNING | SUBSTATE_NORMAL_RUN;
                            break;
                    }
            }
            break;

        case STATE_HOLDING:
            switch (usbHostState & SUBSTATE_MASK)
            {
                case SUBSTATE_HOLD_INIT:
                    // We're here because we cannot communicate with the current device
                    // that is plugged in.  Turn off SOF's and all interrupts except
                    // the DETACH interrupt.
                    #ifdef DEBUG_MODE
                        UART2PrintString( "HOST: Holding.\r\n" );
                    #endif
                    U1CON               = USB_HOST_MODE_ENABLE | USB_SOF_DISABLE;                       // Turn of SOF's to cut down noise
                    U1IE                = 0;
                    U1IR                = 0xFF;
                    U1OTGIE             &= 0x8C;
                    U1OTGIR             = 0x7D;
                    U1EIE               = 0;
                    U1EIR               = 0xFF;
                    U1IEbits.DETACHIE   = 1;

                    switch (usbDeviceInfo.errorCode )
                    {
                        case USB_HOLDING_UNSUPPORTED_HUB:
                            temp = EVENT_HUB_ATTACH;
                            break;

                        case USB_HOLDING_UNSUPPORTED_DEVICE:
                            temp = EVENT_UNSUPPORTED_DEVICE;

                            #ifdef  USB_SUPPORT_OTG
                            //Abort HNP
                            USB_OTGEventHandler (0, OTG_EVENT_HNP_ABORT , 0, 0 );
                            #endif

                            break;

                        case USB_CANNOT_ENUMERATE:
                            temp = EVENT_CANNOT_ENUMERATE;
                            break;

                        case USB_HOLDING_CLIENT_INIT_ERROR:
                            temp = EVENT_CLIENT_INIT_ERROR;
                            break;

                        case USB_HOLDING_OUT_OF_MEMORY:
                            temp = EVENT_OUT_OF_MEMORY;
                            break;

                        default:
                            temp = EVENT_UNSPECIFIED_ERROR; // This should never occur
                            break;
                    }

                    // Report the problem to the application.
                    USB_HOST_APP_EVENT_HANDLER( usbDeviceInfo.deviceAddress, temp, &usbDeviceInfo.currentConfigurationPower , 1 );

                    _USB_SetNextSubState();
                    break;

                case SUBSTATE_HOLD:
                    // Hold here until a DETACH interrupt frees us.
                    break;

                default:
                    break;
            }
            break;
    }

}

/****************************************************************************
  Function:
    void USBHostTerminateTransfer( BYTE deviceAddress, BYTE endpoint )


  Summary:
    This function terminates the current transfer for the given endpoint.

  Description:
    This function terminates the current transfer for the given endpoint.  It
    can be used to terminate reads or writes that the device is not
    responding to.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device address
    BYTE endpoint       - Endpoint number

  Returns:
    None

  Remarks:
    None
  ***************************************************************************/

void USBHostTerminateTransfer( BYTE deviceAddress, BYTE endpoint )
{
    USB_ENDPOINT_INFO *ep;

    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return; // USB_UNKNOWN_DEVICE;
    }

    ep = pEndpointList;
    while (ep != NULL)
    {
        if (ep->bEndpointAddress == endpoint)
        {
            ep->status.bfUserAbort          = 1;
            ep->status.bfTransferComplete   = 1;
            return;
        }
        ep = ep->next;
    }

    return;
}

/****************************************************************************
  Function:
    BOOL USBHostTransferIsComplete( BYTE deviceAddress, BYTE endpoint,
                        BYTE *errorCode, DWORD *byteCount )

  Summary:
    This function initiates whether or not the last endpoint transaction is
    complete.

  Description:
    This function initiates whether or not the last endpoint transaction is
    complete.  If it is complete, an error code and the number of bytes
    transferred are returned.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device address
    BYTE endpoint       - Endpoint number
    BYTE *errorCode     - Error code indicating the status of the transfer.
                            Only valid if the transfer is complete.
    DWORD *byteCount    - The number of bytes sent or received.

  Return Values:
    TRUE    - Transfer is complete.
    FALSE   - Transfer is not complete.

  Remarks:
    Possible values for errorCode are:
        * USB_SUCCESS                     - Transfer successful
        * USB_UNKNOWN_DEVICE              - Device not attached
        * USB_ENDPOINT_STALLED            - Endpoint STALL'd
        * USB_ENDPOINT_ERROR_ILLEGAL_PID  - Illegal PID returned
        * USB_ENDPOINT_ERROR_BIT_STUFF
        * USB_ENDPOINT_ERROR_DMA
        * USB_ENDPOINT_ERROR_TIMEOUT
        * USB_ENDPOINT_ERROR_DATA_FIELD
        * USB_ENDPOINT_ERROR_CRC16
        * USB_ENDPOINT_ERROR_END_OF_FRAME
        * USB_ENDPOINT_ERROR_PID_CHECK
        * USB_ENDPOINT_ERROR              - Other error
  ***************************************************************************/

BOOL USBHostTransferIsComplete( BYTE deviceAddress, BYTE endpoint, BYTE *errorCode,
            DWORD *byteCount )
{
    USB_ENDPOINT_INFO   *ep;
    BYTE                transferComplete;

    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        *errorCode = USB_UNKNOWN_DEVICE;
        *byteCount = 0;
        return TRUE;
    }

    ep = pEndpointList;
    while (ep != NULL)
    {
        if (ep->bEndpointAddress == endpoint)
        {
            // bfTransferComplete, the status flags, and byte count can be
            // changed in an interrupt service routine.  Therefore, we'll
            // grab it first, save it locally, and then determine the rest of
            // the information.  It is better to say that the transfer is not
            // yet complete, since the caller will simply try again.

            // Save off the Transfer Complete status.  That way, we won't
            // load up bad values and then say the transfer is complete.
            transferComplete = ep->status.bfTransferComplete;

            // Set up error code.  This is only valid if the transfer is complete.
            if (ep->status.bfTransferSuccessful)
            {
                *errorCode = USB_SUCCESS;
                *byteCount = ep->dataCount;
            }
            else if (ep->status.bfStalled)
            {
                *errorCode = USB_ENDPOINT_STALLED;
            }
            else if (ep->status.bfError)
            {
                *errorCode = ep->bErrorCode;
            }
            else
            {
                *errorCode = USB_ENDPOINT_UNRESOLVED_STATE;
            }

            return transferComplete;
        }
        ep = ep->next;
    }

    // The endpoint was not found.  Return TRUE so we can return a valid error code.
    *errorCode = USB_ENDPOINT_NOT_FOUND;
    return TRUE;
}

/****************************************************************************
  Function:
    BYTE  USBHostVbusEvent( USB_EVENT vbusEvent, BYTE hubAddress,
                                        BYTE portNumber)

  Summary:
    This function handles Vbus events that are detected by the application.

  Description:
    This function handles Vbus events that are detected by the application.
    Since Vbus management is application dependent, the application is
    responsible for monitoring Vbus and detecting overcurrent conditions
    and removal of the overcurrent condition.  If the application detects
    an overcurrent condition, it should call this function with the event
    EVENT_VBUS_OVERCURRENT with the address of the hub and port number that
    has the condition.  When a port returns to normal operation, the
    application should call this function with the event
    EVENT_VBUS_POWER_AVAILABLE so the stack knows that it can allow devices
    to attach to that port.

  Precondition:
    None

  Parameters:
    USB_EVENT vbusEvent     - Vbus event that occured.  Valid events:
                                    * EVENT_VBUS_OVERCURRENT
                                    * EVENT_VBUS_POWER_AVAILABLE
    BYTE hubAddress         - Address of the hub device (USB_ROOT_HUB for the
                                root hub)
    BYTE portNumber         - Number of the physical port on the hub (0 - based)

  Return Values:
    USB_SUCCESS             - Event handled
    USB_ILLEGAL_REQUEST     - Invalid event, hub, or port

  Remarks:
    None
  ***************************************************************************/

BYTE  USBHostVbusEvent(USB_EVENT vbusEvent, BYTE hubAddress, BYTE portNumber)
{
    if ((hubAddress == USB_ROOT_HUB) &&
        (portNumber == 0 ))
    {
        if (vbusEvent == EVENT_VBUS_OVERCURRENT)
        {
            USBHostShutdown();
            usbRootHubInfo.flags.bPowerGoodPort0 = 0;
            return USB_SUCCESS;
        }
        if (vbusEvent == EVENT_VBUS_POWER_AVAILABLE)
        {
            usbRootHubInfo.flags.bPowerGoodPort0 = 1;
            return USB_SUCCESS;
        }
    }

    return USB_ILLEGAL_REQUEST;
}


/****************************************************************************
  Function:
    BYTE USBHostWrite( BYTE deviceAddress, BYTE endpoint, BYTE *data,
                        DWORD size )

  Summary:
    This function initiates a write to the attached device.

  Description:
    This function initiates a write to the attached device.  The data buffer
    pointed to by *data must remain valid during the entire time that the
    write is taking place; the data is not buffered by the stack.

  Precondition:
    None

  Parameters:
    BYTE deviceAddress  - Device address
    BYTE endpoint       - Endpoint number
    BYTE *data          - Pointer to where the data is stored
    DWORD size          - Number of data bytes to send

  Return Values:
    USB_SUCCESS                     - Write started successfully.
    USB_UNKNOWN_DEVICE              - Device with the specified address not found.
    USB_INVALID_STATE               - We are not in a normal running state.
    USB_ENDPOINT_ILLEGAL_TYPE       - Must use USBHostControlWrite to write
                                        to a control endpoint.
    USB_ENDPOINT_ILLEGAL_DIRECTION  - Must write to an OUT endpoint.
    USB_ENDPOINT_STALLED            - Endpoint is stalled.  Must be cleared
                                        by the application.
    USB_ENDPOINT_ERROR              - Endpoint has too many errors.  Must be
                                        cleared by the application.
    USB_ENDPOINT_BUSY               - A Write is already in progress.
    USB_ENDPOINT_NOT_FOUND          - Invalid endpoint.

  Remarks:
    None
  ***************************************************************************/

BYTE USBHostWrite( BYTE deviceAddress, BYTE endpoint, BYTE *data, DWORD size )
{
    USB_ENDPOINT_INFO *ep;

    // Find the required device
    if (deviceAddress != usbDeviceInfo.deviceAddress)
    {
        return USB_UNKNOWN_DEVICE;
    }

    // If we are not in a normal user running state, we cannot do this.
    if ((usbHostState & STATE_MASK) != STATE_RUNNING)
    {
        return USB_INVALID_STATE;
    }

    ep = pEndpointList;
    while (ep != NULL)
    {
        if (ep->bEndpointAddress == endpoint)
        {
            if (ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_CONTROL)
            {
                // Must not be a control endpoint.
                return USB_ENDPOINT_ILLEGAL_TYPE;
            }

            if (ep->bEndpointAddress & 0x80)
            {
                // Trying to do an OUT with an IN endpoint.
                return USB_ENDPOINT_ILLEGAL_DIRECTION;
            }

            if (ep->status.bfStalled)
            {
                // The endpoint is stalled.  It must be restarted before a write
                // can be performed.
                return USB_ENDPOINT_STALLED;
            }

            if (ep->status.bfError)
            {
                // The endpoint has errored.  The error must be cleared before a
                // write can be performed.
                return USB_ENDPOINT_ERROR;
            }

            if (!ep->status.bfTransferComplete)
            {
                // We are already processing a request for this endpoint.
                return USB_ENDPOINT_BUSY;
            }

            _USB_InitWrite( ep, data, size );

            return USB_SUCCESS;
        }
        ep = ep->next;
    }
    return USB_ENDPOINT_NOT_FOUND;   // Endpoint not found
}


// *****************************************************************************
// *****************************************************************************
// Section: Internal Functions
// *****************************************************************************
// *****************************************************************************

/****************************************************************************
  Function:
    void _USB_CheckCommandAndEnumerationAttempts( void )

  Summary:
    This function is called when we've received a STALL or a NAK when trying
    to enumerate.

  Description:
    This function is called when we've received a STALL or a NAK when trying
    to enumerate.  We allow so many attempts at each command, and so many
    attempts at enumeration.  If the command fails and there are more command
    attempts, we try the command again.  If the command fails and there are
    more enumeration attempts, we reset and try to enumerate again.
    Otherwise, we go to the holding state.

  Precondition:
    usbHostState != STATE_RUNNING

  Parameters:
    None - None

  Returns:
    None

  Remarks:
    None
  ***************************************************************************/

void _USB_CheckCommandAndEnumerationAttempts( void )
{
    #ifdef DEBUG_MODE
        UART2PutChar( '=' );
    #endif

    // Clear the error and stall flags.  A stall here does not require
    // host intervention to clear.
    pCurrentEndpoint->status.bfError    = 0;
    pCurrentEndpoint->status.bfStalled  = 0;

    numCommandTries --;
    if (numCommandTries != 0)
    {
        // We still have retries left on this command.  Try again.
        usbHostState &= ~SUBSUBSTATE_MASK;
    }
    else
    {
        // This command has timed out.
        // We are enumerating.  See if we can try to enumerate again.
        numEnumerationTries --;
        if (numEnumerationTries != 0)
        {
            // We still have retries left to try to enumerate.  Reset and try again.
            usbHostState = STATE_ATTACHED | SUBSTATE_RESET_DEVICE;
        }
        else
        {
            // Give up.  The device is not responding properly.
            _USB_SetErrorCode( USB_CANNOT_ENUMERATE );
            _USB_SetHoldState();
        }
    }
}


/****************************************************************************
  Function:
    BOOL _USB_FindClassDriver( BYTE bClass, BYTE bSubClass, BYTE bProtocol, BYTE *pbClientDrv )

  Summary:


  Description:
    This routine scans the TPL table looking for the entry with
                the given class, subclass, and protocol values.

  Precondition:
    usbTPL must be define by the application.

  Parameters:
    bClass      - The class of the desired entry
    bSubClass   - The subclass of the desired entry
    bProtocol   - The protocol of the desired entry
    pbClientDrv - Returned index to the client driver in the client driver
                    table.

  Return Values:
    TRUE    - A class driver was found.
    FALSE   - A class driver was not found.

  Remarks:
    None
  ***************************************************************************/

BOOL _USB_FindClassDriver( BYTE bClass, BYTE bSubClass, BYTE bProtocol, BYTE *pbClientDrv )
{
    int i;

    i = 0;
    while (i < NUM_TPL_ENTRIES)
    {
        if ((usbTPL[i].flags.bfIsClassDriver == 1        ) &&
            (usbTPL[i].device.bClass         == bClass   ) &&
            (usbTPL[i].device.bSubClass      == bSubClass) &&
            (usbTPL[i].device.bProtocol      == bProtocol)   )
        {
            *pbClientDrv = usbTPL[i].ClientDriver;
            #ifdef DEBUG_MODE
                UART2PrintString( "HOST: Client driver found.\r\n" );
            #endif
            return TRUE;
        }
        i++;
    }

    #ifdef DEBUG_MODE
        UART2PrintString( "HOST: Client driver NOT found.\r\n" );
    #endif
    return FALSE;

} // _USB_FindClassDriver


/****************************************************************************
  Function:
    BOOL _USB_FindDeviceLevelClientDriver( void )

  Description:
    This function searches the TPL to try to find a device-level client
    driver.

  Precondition:
    * usbHostState == STATE_ATTACHED|SUBSTATE_VALIDATE_VID_PID
    * usbTPL must be define by the application.

  Parameters:
    None - None

  Return Values:
    TRUE    - Client driver found
    FALSE   - Client driver not found

  Remarks:
    If successful, this function preserves the client's index from the client
    driver table and sets flags indicating that the device should use a
    single client driver.
  ***************************************************************************/

BOOL _USB_FindDeviceLevelClientDriver( void )
{
    WORD                   i;
    USB_DEVICE_DESCRIPTOR *pDesc = (USB_DEVICE_DESCRIPTOR *)pDeviceDescriptor;

    // Scan TPL
    i = 0;
    usbDeviceInfo.flags.bfUseDeviceClientDriver = 0;
    while (i < NUM_TPL_ENTRIES)
    {
        if (usbTPL[i].flags.bfIsClassDriver)
        {
            // Check for a device-class client driver
            if ((usbTPL[i].device.bClass    == pDesc->bDeviceClass   ) &&
                (usbTPL[i].device.bSubClass == pDesc->bDeviceSubClass) &&
                (usbTPL[i].device.bProtocol == pDesc->bDeviceProtocol)   )
            {
                #ifdef DEBUG_MODE
                    UART2PrintString( "HOST: Device validated by class\r\n" );
                #endif
                usbDeviceInfo.flags.bfUseDeviceClientDriver = 1;
            }
        }
        else
        {
            // Check for a device-specific client driver by VID & PID
            #ifdef ALLOW_GLOBAL_VID_AND_PID
            if (((usbTPL[i].device.idVendor  == pDesc->idVendor ) &&
                 (usbTPL[i].device.idProduct == pDesc->idProduct)) ||
                ((usbTPL[i].device.idVendor  == 0xFFFF) &&
                 (usbTPL[i].device.idProduct == 0xFFFF)))
            #else
            if ((usbTPL[i].device.idVendor  == pDesc->idVendor ) &&
                (usbTPL[i].device.idProduct == pDesc->idProduct)   )
            #endif
            {
                #ifdef DEBUG_MODE
                    UART2PrintString( "HOST: Device validated by VID/PID\r\n" );
                #endif
                usbDeviceInfo.flags.bfUseDeviceClientDriver = 1;
            }
        }

        if (usbDeviceInfo.flags.bfUseDeviceClientDriver)
        {
            // Save client driver info
            usbDeviceInfo.deviceClientDriver = usbTPL[i].ClientDriver;

            // Select configuration if it is given in the TPL
            if (usbTPL[i].flags.bfSetConfiguration)
            {
                usbDeviceInfo.currentConfiguration = usbTPL[i].bConfiguration;
            }

            return TRUE;
        }

        i++;
    }

    #ifdef DEBUG_MODE
        UART2PrintString( "HOST: Device not yet validated\r\n" );
    #endif

    return FALSE;
}


/****************************************************************************
  Function:
    USB_INTERFACE_INFO * _USB_FindInterface ( BYTE bInterface, BYTE bAltSetting )

  Description:
    This routine scans the interface linked list and returns a pointer to the
    node identified by the interface and alternate setting.

  Precondition:
    None

  Parameters:
    bInterface  - Interface number
    bAltSetting - Interface alternate setting number

  Returns:
    USB_INTERFACE_INFO *  - Pointer to the interface linked list node.

  Remarks:
    None
  ***************************************************************************/

USB_INTERFACE_INFO * _USB_FindInterface ( BYTE bInterface, BYTE bAltSetting )
{
    USB_INTERFACE_INFO *pCurIntf = pInterfaceList;

    while (pCurIntf)
    {
        if (pCurIntf->interface           == bInterface &&
            pCurIntf->interfaceAltSetting == bAltSetting  )
        {
            return pCurIntf;
        }
    }

    return NULL;

} // _USB_FindInterface


/****************************************************************************
  Function:
    void _USB_FindNextToken( void )

  Description:
    This function determines the next token to send of all current pending
    transfers.

  Precondition:
    None

  Parameters:
    None - None

  Return Values:
    TRUE    - A token was sent
    FALSE   - No token was found to send, so the routine can be called again.

  Remarks:
    This routine is only called from an interrupt handler, either SOFIF or
    TRNIF.
  ***************************************************************************/

void _USB_FindNextToken( void )
{
    #ifdef USB_SUPPORT_BULK_TRANSFERS
        BYTE            countBulkTransactions;
    #endif
    USB_ENDPOINT_INFO   *ep;
    BOOL                illegalState = FALSE;

    // If the device is suspended or resuming, do not send any tokens.  We will
    // send the next token on an SOF interrupt after the resume recovery time
    // has expired.
    if ((usbHostState & (SUBSTATE_MASK | SUBSUBSTATE_MASK)) == (STATE_RUNNING | SUBSTATE_SUSPEND_AND_RESUME))
    {
        return;
    }

    // If we are currently sending a token, we cannot do anything.  We will come
    // back in here when we get either the Token Done or the Start of Frame interrupt.
    if (usbBusInfo.flags.bfTokenAlreadyWritten) //(U1CONbits.TOKBUSY)
    {
        return;
    }

    // We will handle control transfers first.  We only allow one control
    // transfer per frame.
    if (!usbBusInfo.flags.bfControlTransfersDone)
    {
        // Look for any control transfers.
        ep = pEndpointList;
        while (ep != NULL)
        {
            if ((ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_CONTROL) &&
                !ep->status.bfTransferComplete)
            {
                // This endpoint is ready for servicing.
                pCurrentEndpoint = ep;
                switch (ep->transferState & TSTATE_MASK)
                {
                    case TSTATE_CONTROL_NO_DATA:
                        switch (ep->transferState & TSUBSTATE_MASK)
                        {
                            case TSUBSTATE_CONTROL_NO_DATA_SETUP:
                                _USB_SetDATA01( DTS_DATA0 );
                                _USB_SetBDT( USB_TOKEN_SETUP );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_SETUP );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_NO_DATA_ACK:
                                pCurrentEndpoint->dataCountMax = pCurrentEndpoint->dataCount;
                                _USB_SetDATA01( DTS_DATA1 );
                                _USB_SetBDT( USB_TOKEN_IN );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_IN );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_NO_DATA_COMPLETE:
                                ep->transferState               = TSTATE_IDLE;
                                ep->status.bfTransferComplete   = 1;
                                #if defined( USB_ENABLE_TRANSFER_EVENT )
                                    if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                    {
                                        USB_EVENT_DATA *data;

                                        data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                        data->event = EVENT_TRANSFER;
                                        data->TransferData.dataCount        = ep->dataCount;
                                        data->TransferData.pUserData        = ep->pUserData;
                                        data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                        data->TransferData.bErrorCode       = USB_SUCCESS;
                                        data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                    }
                                    else
                                    {
                                        ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                    }
                                #endif
                        break;

                            case TSUBSTATE_ERROR:
                                ep->transferState               = TSTATE_IDLE;
                                ep->status.bfTransferComplete   = 1;
                                #if defined( USB_ENABLE_TRANSFER_EVENT )
                                    if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                    {
                                        USB_EVENT_DATA *data;

                                        data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                        data->event = EVENT_BUS_ERROR;
                                        data->TransferData.dataCount        = 0;
                                        data->TransferData.pUserData        = NULL;
                                        data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                        data->TransferData.bErrorCode       = ep->bErrorCode;
                                        data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                    }
                                    else
                                    {
                                        ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                    }
                                #endif
                                break;

                            default:
                                illegalState = TRUE;
                                break;
                        }
                        break;

                    case TSTATE_CONTROL_READ:
                        switch (ep->transferState & TSUBSTATE_MASK)
                        {
                            case TSUBSTATE_CONTROL_READ_SETUP:
                                _USB_SetDATA01( DTS_DATA0 );
                                _USB_SetBDT( USB_TOKEN_SETUP );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_SETUP );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_READ_DATA:
                                _USB_SetBDT( USB_TOKEN_IN );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_IN );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_READ_ACK:
                                pCurrentEndpoint->dataCountMax = pCurrentEndpoint->dataCount;
                                _USB_SetDATA01( DTS_DATA1 );
                                _USB_SetBDT( USB_TOKEN_OUT );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_OUT );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_READ_COMPLETE:
                                ep->transferState               = TSTATE_IDLE;
                                ep->status.bfTransferComplete   = 1;
                                #if defined( USB_ENABLE_TRANSFER_EVENT )
                                    if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                    {
                                        USB_EVENT_DATA *data;

                                        data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                        data->event = EVENT_TRANSFER;
                                        data->TransferData.dataCount        = ep->dataCount;
                                        data->TransferData.pUserData        = ep->pUserData;
                                        data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                        data->TransferData.bErrorCode       = USB_SUCCESS;
                                        data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                    }
                                    else
                                    {
                                        ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                    }
                                #endif
                                break;

                            case TSUBSTATE_ERROR:
                                ep->transferState               = TSTATE_IDLE;
                                ep->status.bfTransferComplete   = 1;
                                #if defined( USB_ENABLE_TRANSFER_EVENT )
                                    if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                    {
                                        USB_EVENT_DATA *data;

                                        data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                        data->event = EVENT_BUS_ERROR;
                                        data->TransferData.dataCount        = 0;
                                        data->TransferData.pUserData        = NULL;
                                        data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                        data->TransferData.bErrorCode       = ep->bErrorCode;
                                        data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                    }
                                    else
                                    {
                                        ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                    }
                                #endif
                                break;

                            default:
                                illegalState = TRUE;
                                break;
                        }
                        break;

                    case TSTATE_CONTROL_WRITE:
                        switch (ep->transferState & TSUBSTATE_MASK)
                        {
                            case TSUBSTATE_CONTROL_WRITE_SETUP:
                                _USB_SetDATA01( DTS_DATA0 );
                                _USB_SetBDT( USB_TOKEN_SETUP );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_SETUP );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_WRITE_DATA:
                                _USB_SetBDT( USB_TOKEN_OUT );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_OUT );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_WRITE_ACK:
                                pCurrentEndpoint->dataCountMax = pCurrentEndpoint->dataCount;
                                _USB_SetDATA01( DTS_DATA1 );
                                _USB_SetBDT( USB_TOKEN_IN );
                                _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_IN );
                                #ifdef ONE_CONTROL_TRANSACTION_PER_FRAME
                                    usbBusInfo.flags.bfControlTransfersDone = 1; // Only one control transfer per frame.
                                #endif
                                return;
                                break;

                            case TSUBSTATE_CONTROL_WRITE_COMPLETE:
                                ep->transferState               = TSTATE_IDLE;
                                ep->status.bfTransferComplete   = 1;
                                #if defined( USB_ENABLE_TRANSFER_EVENT )
                                    if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                    {
                                        USB_EVENT_DATA *data;

                                        data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                        data->event = EVENT_TRANSFER;
                                        data->TransferData.dataCount        = ep->dataCount;
                                        data->TransferData.pUserData        = ep->pUserData;
                                        data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                        data->TransferData.bErrorCode       = USB_SUCCESS;
                                        data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                    }
                                    else
                                    {
                                        ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                    }
                                #endif
                                break;

                            case TSUBSTATE_ERROR:
                                ep->transferState               = TSTATE_IDLE;
                                ep->status.bfTransferComplete   = 1;
                                #if defined( USB_ENABLE_TRANSFER_EVENT )
                                    if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                    {
                                        USB_EVENT_DATA *data;

                                        data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                        data->event = EVENT_BUS_ERROR;
                                        data->TransferData.dataCount        = 0;
                                        data->TransferData.pUserData        = NULL;
                                        data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                        data->TransferData.bErrorCode       = ep->bErrorCode;
                                        data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                    }
                                    else
                                    {
                                        ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                    }
                                #endif
                                break;

                            default:
                                illegalState = TRUE;
                                break;
                        }
                        break;

                    default:
                        illegalState = TRUE;
                }

                if (illegalState)
                {
                    // We should never use this, but in case we do, put the endpoint
                    // in a recoverable state.
                    ep->transferState               = TSTATE_IDLE;
                    ep->status.bfTransferComplete   = 1;
                }
            }
            ep = ep->next;
        }

        // If we've gone through all the endpoints, we do not have any more control transfers.
        usbBusInfo.flags.bfControlTransfersDone = 1;
    }

    #ifdef USB_SUPPORT_ISOCHRONOUS_TRANSFERS
        // Next, we will handle isochronous transfers.  We must be careful with
        // these.  The maximum packet size for an isochronous transfer is 1023
        // bytes, so we cannot use the threshold register (U1SOF) to ensure that
        // we do not write too many tokens during a frame.  Instead, we must count
        // the number of bytes we are sending and stop sending isochronous
        // transfers when we reach that limit.

        // TODO: Implement scheduling by using usbBusInfo.dBytesSentInFrame

        if (!usbBusInfo.flags.bfIsochronousTransfersDone)
        {
            // Look for any isochronous operations.
            ep = pEndpointList;
            while (ep != NULL)
            {
                if (ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_ISOCHRONOUS)
                {
                    if (ep->status.bfTransferComplete)
                    {
                        // The endpoint doesn't need servicing.  If the interval count
                        // has reached 0 and the user has not initiated another transaction,
                        // reset the interval count for the next interval.
                        if (ep->wIntervalCount == 0)
                        {
                            // Reset the interval count for the next packet.
                            ep->wIntervalCount = ep->wInterval;
                        }
                    }
                    else
                    {
                        // This endpoint might be ready for servicing.
                        pCurrentEndpoint = ep;

                        switch (ep->transferState & TSTATE_MASK)
                        {
                            case TSTATE_ISOCHRONOUS_READ:
                                switch (ep->transferState & TSUBSTATE_MASK)
                                {
                                    case TSUBSTATE_ISOCHRONOUS_READ_DATA:
                                        if (ep->wIntervalCount == 0)
                                        {
                                            // Reset the interval count for the next packet.
                                            ep->wIntervalCount = ep->wInterval;

                                            _USB_SetDATA01( DTS_DATA0 );    // Always DATA0 for isochronous
                                            _USB_SetBDT( USB_TOKEN_IN );
                                            _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_IN );
                                            return;
                                        }
                                        break;

                                    case TSUBSTATE_ISOCHRONOUS_READ_COMPLETE:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_TRANSFER;
                                                data->TransferData.dataCount        = ep->dataCount;
                                                data->TransferData.pUserData        = ep->pUserData;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = USB_SUCCESS;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    case TSUBSTATE_ERROR:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_BUS_ERROR;
                                                data->TransferData.dataCount        = 0;
                                                data->TransferData.pUserData        = NULL;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = ep->bErrorCode;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    default:
                                        illegalState = TRUE;
                                        break;
                                }
                                break;

                            case TSTATE_ISOCHRONOUS_WRITE:
                                switch (ep->transferState & TSUBSTATE_MASK)
                                {
                                    case TSUBSTATE_ISOCHRONOUS_WRITE_DATA:
                                        if (ep->wIntervalCount == 0)
                                        {
                                            // Reset the interval count for the next packet.
                                            ep->wIntervalCount = ep->wInterval;

                                            _USB_SetDATA01( DTS_DATA0 );    // Always DATA0 for isochronous
                                            _USB_SetBDT( USB_TOKEN_OUT );
                                            _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_OUT );
                                            return;
                                        }
                                        break;

                                    case TSUBSTATE_ISOCHRONOUS_WRITE_COMPLETE:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_TRANSFER;
                                                data->TransferData.dataCount        = ep->dataCount;
                                                data->TransferData.pUserData        = ep->pUserData;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = USB_SUCCESS;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    case TSUBSTATE_ERROR:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_BUS_ERROR;
                                                data->TransferData.dataCount        = 0;
                                                data->TransferData.pUserData        = NULL;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = ep->bErrorCode;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    default:
                                        illegalState = TRUE;
                                        break;
                                }
                                break;

                            default:
                                illegalState = TRUE;
                                break;
                        }

                        if (illegalState)
                        {
                            // We should never use this, but in case we do, put the endpoint
                            // in a recoverable state.
                            ep->transferState               = TSTATE_IDLE;
                            ep->status.bfTransferComplete   = 1;
                        }
                    }
                }
                ep = ep->next;
            }

            // If we've gone through all the endpoints, we do not have any more isochronous transfers.
            usbBusInfo.flags.bfIsochronousTransfersDone = 1;
        }
    #endif

    #ifdef USB_SUPPORT_INTERRUPT_TRANSFERS
        if (!usbBusInfo.flags.bfInterruptTransfersDone)
        {
            // Look for any interrupt operations.
            ep = pEndpointList;
            while (ep != NULL)
            {
                if (ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_INTERRUPT)
                {
                    if (ep->status.bfTransferComplete)
                    {
                        // The endpoint doesn't need servicing.  If the interval count
                        // has reached 0 and the user has not initiated another transaction,
                        // reset the interval count for the next interval.
                        if (ep->wIntervalCount == 0)
                        {
                            // Reset the interval count for the next packet.
                            ep->wIntervalCount = ep->wInterval;
                        }
                    }
                    else
                    {
                        // This endpoint might be ready for servicing.
                        pCurrentEndpoint = ep;

                        switch (ep->transferState & TSTATE_MASK)
                        {
                            case TSTATE_INTERRUPT_READ:
                                switch (ep->transferState & TSUBSTATE_MASK)
                                {
                                    case TSUBSTATE_INTERRUPT_READ_DATA:
                                        if (ep->wIntervalCount == 0)
                                        {
                                            // Reset the interval count for the next packet.
                                            ep->wIntervalCount = ep->wInterval;

                                            _USB_SetBDT( USB_TOKEN_IN );
                                            _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_IN );
                                            return;
                                        }
                                        break;

                                    case TSUBSTATE_INTERRUPT_READ_COMPLETE:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_TRANSFER;
                                                data->TransferData.dataCount        = ep->dataCount;
                                                data->TransferData.pUserData        = ep->pUserData;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = USB_SUCCESS;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    case TSUBSTATE_ERROR:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_BUS_ERROR;
                                                data->TransferData.dataCount        = 0;
                                                data->TransferData.pUserData        = NULL;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = ep->bErrorCode;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    default:
                                        illegalState = TRUE;
                                        break;
                                }
                                break;

                            case TSTATE_INTERRUPT_WRITE:
                                switch (ep->transferState & TSUBSTATE_MASK)
                                {
                                    case TSUBSTATE_INTERRUPT_WRITE_DATA:
                                        if (ep->wIntervalCount == 0)
                                        {
                                            // Reset the interval count for the next packet.
                                            ep->wIntervalCount = ep->wInterval;

                                            _USB_SetBDT( USB_TOKEN_OUT );
                                            _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_OUT );
                                            return;
                                        }
                                        break;

                                    case TSUBSTATE_INTERRUPT_WRITE_COMPLETE:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_TRANSFER;
                                                data->TransferData.dataCount        = ep->dataCount;
                                                data->TransferData.pUserData        = ep->pUserData;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = USB_SUCCESS;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    case TSUBSTATE_ERROR:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_BUS_ERROR;
                                                data->TransferData.dataCount        = 0;
                                                data->TransferData.pUserData        = NULL;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = ep->bErrorCode;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    default:
                                        illegalState = TRUE;
                                        break;
                                }
                                break;

                            default:
                                illegalState = TRUE;
                                break;
                        }

                        if (illegalState)
                        {
                            // We should never use this, but in case we do, put the endpoint
                            // in a recoverable state.
                            ep->status.bfTransferComplete   = 1;
                            ep->transferState               = TSTATE_IDLE;
                        }
                    }
                }
                ep = ep->next;
            }

            // If we've gone through all the endpoints, we do not have any more interrupt transfers.
            usbBusInfo.flags.bfInterruptTransfersDone = 1;
        }
    #endif

    #ifdef USB_SUPPORT_BULK_TRANSFERS
#ifdef ALLOW_MULTIPLE_BULK_TRANSACTIONS_PER_FRAME
TryBulk:
#endif

        if (!usbBusInfo.flags.bfBulkTransfersDone)
        {
            #ifndef ALLOW_MULTIPLE_BULK_TRANSACTIONS_PER_FRAME
                // Only go through this section once if we are not allowing multiple transactions
                // per frame.
                usbBusInfo.flags.bfBulkTransfersDone = 1;
            #endif

            // Look for any bulk operations.  Try to service all pending requests within the frame.
            ep = pEndpointList;
            countBulkTransactions = 0;

            while (ep != NULL)
            {
#ifdef ALLOW_MULTIPLE_NAKS_PER_FRAME
                if ((ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_BULK) &&
                    !ep->status.bfTransferComplete)
#else
                if ((ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_BULK) &&
                    !ep->status.bfTransferComplete &&
                    !ep->status.bfLastTransferNAKd)
#endif
                {
                    countBulkTransactions ++;
                    if (countBulkTransactions > usbBusInfo.lastBulkTransaction)
                    {
                        // This endpoint is ready for servicing.
                        pCurrentEndpoint = ep;
                        usbBusInfo.lastBulkTransaction = countBulkTransactions;

                        switch (ep->transferState & TSTATE_MASK)
                        {
                            case TSTATE_BULK_READ:
                                switch (ep->transferState & TSUBSTATE_MASK)
                                {
                                    case TSUBSTATE_BULK_READ_DATA:
                                        _USB_SetBDT( USB_TOKEN_IN );
                                        _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_IN );
                                        return;
                                        break;

                                    case TSUBSTATE_BULK_READ_COMPLETE:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_TRANSFER;
                                                data->TransferData.dataCount        = ep->dataCount;
                                                data->TransferData.pUserData        = ep->pUserData;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = USB_SUCCESS;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    case TSUBSTATE_ERROR:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_BUS_ERROR;
                                                data->TransferData.dataCount        = 0;
                                                data->TransferData.pUserData        = NULL;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = ep->bErrorCode;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    default:
                                        illegalState = TRUE;
                                        break;
                                }
                                break;

                            case TSTATE_BULK_WRITE:
                                switch (ep->transferState & TSUBSTATE_MASK)
                                {
                                    case TSUBSTATE_BULK_WRITE_DATA:
                                        _USB_SetBDT( USB_TOKEN_OUT );
                                        _USB_SendToken( ep->bEndpointAddress, USB_TOKEN_OUT );
                                        return;
                                        break;

                                    case TSUBSTATE_BULK_WRITE_COMPLETE:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_TRANSFER;
                                                data->TransferData.dataCount        = ep->dataCount;
                                                data->TransferData.pUserData        = ep->pUserData;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = USB_SUCCESS;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    case TSUBSTATE_ERROR:
                                        ep->transferState               = TSTATE_IDLE;
                                        ep->status.bfTransferComplete   = 1;
                                        #if defined( USB_ENABLE_TRANSFER_EVENT )
                                            if (StructQueueIsNotFull(&usbEventQueue, USB_EVENT_QUEUE_DEPTH))
                                            {
                                                USB_EVENT_DATA *data;

                                                data = StructQueueAdd(&usbEventQueue, USB_EVENT_QUEUE_DEPTH);
                                                data->event = EVENT_BUS_ERROR;
                                                data->TransferData.dataCount        = 0;
                                                data->TransferData.pUserData        = NULL;
                                                data->TransferData.bEndpointAddress = ep->bEndpointAddress;
                                                data->TransferData.bErrorCode       = ep->bErrorCode;
                                                data->TransferData.bmAttributes.val = ep->bmAttributes.val;
                                            }
                                            else
                                            {
                                                ep->bmAttributes.val = USB_EVENT_QUEUE_FULL;
                                            }
                                        #endif
                                        break;

                                    default:
                                        illegalState = TRUE;
                                        break;
                                }
                                break;

                            default:
                                illegalState = TRUE;
                                break;
                        }

                        if (illegalState)
                        {
                            // We should never use this, but in case we do, put the endpoint
                            // in a recoverable state.
                            ep->transferState               = TSTATE_IDLE;
                            ep->status.bfTransferComplete   = 1;
                        }
                    }
                }
                ep = ep->next;
            }

            // We've gone through all the bulk transactions, but we have time for more.
            // If we have any bulk transactions, go back to the beginning of the list
            // and start over.
            #ifdef ALLOW_MULTIPLE_BULK_TRANSACTIONS_PER_FRAME
                if (countBulkTransactions)
                {
                    usbBusInfo.lastBulkTransaction = 0;
                    goto TryBulk;

                }
            #endif

            // If we've gone through all the endpoints, we do not have any more bulk transfers.
            usbBusInfo.flags.bfBulkTransfersDone = 1;
        }
    #endif

    return;
}


/****************************************************************************
  Function:
    void _USB_FreeConfigMemory( void )

  Description:
    This function frees the interface and endpoint lists associated
                with a configuration.

  Precondition:
    None

  Parameters:
    None - None

  Returns:
    None

  Remarks:
    The EP 0 block is retained.
  ***************************************************************************/

void _USB_FreeConfigMemory( void )
{
    BYTE    *pTemp;

    while (pInterfaceList != NULL)
    {
        pTemp = (BYTE *)pInterfaceList->next;
        free(pInterfaceList);
        pInterfaceList = (USB_INTERFACE_INFO *)pTemp;
    }

    if (pEndpointList != NULL) // Should not be null!
    {
        while (pEndpointList->next != NULL) // Leave EP0 intact
        {
            pTemp = (BYTE *)pEndpointList->next->next;
            free( pEndpointList->next );
            pEndpointList->next = (USB_ENDPOINT_INFO *)pTemp;
        }
        pEndpointList->next = NULL;
    }
    pCurrentEndpoint = pEndpointList;

} // _USB_FreeConfigMemory


/****************************************************************************
  Function:
    void _USB_FreeMemory( void )

  Description:
    This function frees all memory that can be freed.  Only the EP0
    information block is retained.

  Precondition:
    None

  Parameters:
    None - None

  Returns:
    None

  Remarks:
    None
  ***************************************************************************/

void _USB_FreeMemory( void )
{
    BYTE    *pTemp;

    while (pConfigurationDescriptorList != NULL)
    {
        pTemp = (BYTE *)pConfigurationDescriptorList->next;
        free( pConfigurationDescriptorList->descriptor );
        free( pConfigurationDescriptorList );
        pConfigurationDescriptorList = (USB_CONFIGURATION *)pTemp;
    }
    if (pDeviceDescriptor != NULL)
    {
        freez( pDeviceDescriptor );
    }
    if (pEP0Data != NULL)
    {
        freez( pEP0Data );
    }

    _USB_FreeConfigMemory();

}


/****************************************************************************
  Function:
    void _USB_InitControlRead( USB_ENDPOINT_INFO *pEndpoint,
                        BYTE *pControlData, WORD controlSize, BYTE *pData,
                        WORD size )

  Description:
    This function sets up the endpoint information for a control (SETUP)
    transfer that will read information.

  Precondition:
    All error checking must be done prior to calling this function.

  Parameters:
    USB_ENDPOINT_INFO *pEndpoint    - Points to the desired endpoint
                                        in the endpoint information list.
    BYTE *pControlData              - Points to the SETUP message.
    WORD controlSize                - Size of the SETUP message.
    BYTE *pData                     - Points to where the read data
                                        is to be stored.
    WORD size                       - Number of data bytes to read.

  Returns:
    None

  Remarks:
    Since endpoint servicing is interrupt driven, the bfTransferComplete
    flag must be set last.
  ***************************************************************************/

void _USB_InitControlRead( USB_ENDPOINT_INFO *pEndpoint, BYTE *pControlData, WORD controlSize,
                            BYTE *pData, WORD size )
{
    pEndpoint->status.bfStalled             = 0;
    pEndpoint->status.bfError               = 0;
    pEndpoint->status.bfUserAbort           = 0;
    pEndpoint->status.bfTransferSuccessful  = 0;
    pEndpoint->status.bfErrorCount          = 0;
    pEndpoint->status.bfLastTransferNAKd    = 0;
    pEndpoint->pUserData                    = pData;
    pEndpoint->dataCount                    = 0;
    pEndpoint->dataCountMax                 = size;
    pEndpoint->countNAKs                    = 0;

    pEndpoint->pUserDataSETUP               = pControlData;
    pEndpoint->dataCountMaxSETUP            = controlSize;
    pEndpoint->transferState                = TSTATE_CONTROL_READ;

    // Set the flag last so all the parameters are set for an interrupt.
    pEndpoint->status.bfTransferComplete    = 0;
}


/****************************************************************************
  Function:
    void _USB_InitControlWrite( USB_ENDPOINT_INFO *pEndpoint,
                        BYTE *pControlData, WORD controlSize, BYTE *pData,
                        WORD size )

  Description:
    This function sets up the endpoint information for a control (SETUP)
    transfer that will write information.

  Precondition:
    All error checking must be done prior to calling this function.

  Parameters:
    USB_ENDPOINT_INFO *pEndpoint    - Points to the desired endpoint
                                                      in the endpoint information list.
    BYTE *pControlData              - Points to the SETUP message.
    WORD controlSize                - Size of the SETUP message.
    BYTE *pData                     - Points to where the write data
                                                      is to be stored.
    WORD size                       - Number of data bytes to write.

  Returns:
    None

  Remarks:
    Since endpoint servicing is interrupt driven, the bfTransferComplete
    flag must be set last.
  ***************************************************************************/

void _USB_InitControlWrite( USB_ENDPOINT_INFO *pEndpoint, BYTE *pControlData,
                WORD controlSize, BYTE *pData, WORD size )
{
    pEndpoint->status.bfStalled             = 0;
    pEndpoint->status.bfError               = 0;
    pEndpoint->status.bfUserAbort           = 0;
    pEndpoint->status.bfTransferSuccessful  = 0;
    pEndpoint->status.bfErrorCount          = 0;
    pEndpoint->status.bfLastTransferNAKd    = 0;
    pEndpoint->pUserData                    = pData;
    pEndpoint->dataCount                    = 0;
    pEndpoint->dataCountMax                 = size;
    pEndpoint->countNAKs                    = 0;

    pEndpoint->pUserDataSETUP               = pControlData;
    pEndpoint->dataCountMaxSETUP            = controlSize;

    if (size == 0)
    {
        pEndpoint->transferState    = TSTATE_CONTROL_NO_DATA;
    }
    else
    {
        pEndpoint->transferState    = TSTATE_CONTROL_WRITE;
    }

    // Set the flag last so all the parameters are set for an interrupt.
    pEndpoint->status.bfTransferComplete    = 0;
}


/****************************************************************************
  Function:
    void _USB_InitRead( USB_ENDPOINT_INFO *pEndpoint, BYTE *pData,
                        WORD size )

  Description:
    This function sets up the endpoint information for an interrupt,
    isochronous, or bulk read.

  Precondition:
    All error checking must be done prior to calling this function.

  Parameters:
    USB_ENDPOINT_INFO *pEndpoint  - Points to the desired endpoint in the
                                    endpoint information list.
    BYTE *pData                   - Points to where the data is to be
                                    stored.
    WORD size                     - Number of data bytes to read.

  Returns:
    None

  Remarks:
    * Control reads should use the routine _USB_InitControlRead().  Since
        endpoint servicing is interrupt driven, the bfTransferComplete flag
        must be set last.

    * For interrupt and isochronous endpoints, we let the interval count
        free run.  The transaction will begin when the interval count
        reaches 0.
  ***************************************************************************/

void _USB_InitRead( USB_ENDPOINT_INFO *pEndpoint, BYTE *pData, WORD size )
{
    pEndpoint->status.bfUserAbort           = 0;
    pEndpoint->status.bfTransferSuccessful  = 0;
    pEndpoint->status.bfErrorCount          = 0;
    pEndpoint->status.bfLastTransferNAKd    = 0;
    pEndpoint->pUserData                    = pData;
    pEndpoint->dataCount                    = 0;
    pEndpoint->dataCountMax                 = size;
    pEndpoint->countNAKs                    = 0;

    if (pEndpoint->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_INTERRUPT)
    {
        pEndpoint->transferState            = TSTATE_INTERRUPT_READ;
    }
    else if (pEndpoint->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_ISOCHRONOUS)
    {
        pEndpoint->transferState            = TSTATE_ISOCHRONOUS_READ;
    }
    else // Bulk
    {
        pEndpoint->transferState            = TSTATE_BULK_READ;
    }

    // Set the flag last so all the parameters are set for an interrupt.
    pEndpoint->status.bfTransferComplete    = 0;
}

/****************************************************************************
  Function:
    void _USB_InitWrite( USB_ENDPOINT_INFO *pEndpoint, BYTE *pData,
                            WORD size )

  Description:
    This function sets up the endpoint information for an interrupt,
                isochronous, or bulk  write.

  Precondition:
    All error checking must be done prior to calling this function.

  Parameters:
    USB_ENDPOINT_INFO *pEndpoint  - Points to the desired endpoint in the
                                    endpoint information list.
    BYTE *pData                   - Points to where the data to send is
                                    stored.
    WORD size                     - Number of data bytes to write.

  Returns:
    None

  Remarks:
    * Control writes should use the routine _USB_InitControlWrite().
                    Since endpoint servicing is interrupt driven, the
                    bfTransferComplete flag must be set last.

                    * For interrupt and isochronous endpoints, we let the interval
                    count free run.  The transaction will begin when the interval
                count reaches 0.
  ***************************************************************************/

void _USB_InitWrite( USB_ENDPOINT_INFO *pEndpoint, BYTE *pData, WORD size )
{
    pEndpoint->status.bfUserAbort           = 0;
    pEndpoint->status.bfTransferSuccessful  = 0;
    pEndpoint->status.bfErrorCount          = 0;
    pEndpoint->status.bfLastTransferNAKd    = 0;
    pEndpoint->pUserData                    = pData;
    pEndpoint->dataCount                    = 0;
    pEndpoint->dataCountMax                 = size;
    pEndpoint->countNAKs                    = 0;

    if (pEndpoint->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_INTERRUPT)
    {
        pEndpoint->transferState            = TSTATE_INTERRUPT_WRITE;
    }
    else if (pEndpoint->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_ISOCHRONOUS)
    {
        pEndpoint->transferState            = TSTATE_ISOCHRONOUS_WRITE;
    }
    else // Bulk
    {
        pEndpoint->transferState            = TSTATE_BULK_WRITE;
    }

    // Set the flag last so all the parameters are set for an interrupt.
    pEndpoint->status.bfTransferComplete    = 0;
}


/****************************************************************************
  Function:
    void _USB_NotifyClients( BYTE address, USB_EVENT event, void *data,
                unsigned int size )

  Description:
    This routine notifies all active client drivers for the given device of
    the given event.

  Precondition:
    None

  Parameters:
    BYTE address        - Address of the device generating the event
    USB_EVENT event     - Event ID
    void *data          - Pointer to event data
    unsigned int size   - Size of data pointed to by data

  Returns:
    None

  Remarks:
    When this driver is modified to support multiple devices, this function
    will require modification.
  ***************************************************************************/

void _USB_NotifyClients( BYTE address, USB_EVENT event, void *data, unsigned int size )
{
    USB_ENDPOINT_INFO   *pEndPoint  = pEndpointList;
    USB_INTERFACE_INFO  *pInterface = pInterfaceList;

    // This is a special case for EP0 on a "composite" device.  In this
    // case, we send the event to all client drivers, because we don't know which
    // one sent the original request.
    if (((event == EVENT_TRANSFER) || (event == EVENT_BUS_ERROR)) &&
        (((HOST_TRANSFER_DATA *)data)->bEndpointAddress == 0))
    {
        USB_INTERFACE_INFO *pInterface = pInterfaceList;

        while (pInterface != NULL)
        {
            usbClientDrvTable[pInterface->clientDriver].EventHandler(address, event, data, size);
            pInterface = pInterface->next;
        }
        return;
    }


    // Send the event to the appropriate driver(s)
    if (usbDeviceInfo.flags.bfUseDeviceClientDriver)    // Driver is at the device level.
    {
        usbClientDrvTable[usbDeviceInfo.deviceClientDriver].EventHandler(address, event, data, size);
    }
    else
    {
        // Some events go to all drivers, some only to specific drivers.
        switch(event)
        {
        case EVENT_TRANSFER:
        case EVENT_BUS_ERROR:
            while (pEndPoint != NULL)   // Scan the endpoint list for the driver for the endpoint that had the event
            {
                if ( pEndPoint->pInterface != NULL &&
                     pEndPoint->bEndpointAddress == ((HOST_TRANSFER_DATA *)data)->bEndpointAddress )
                {
                    usbClientDrvTable[pEndPoint->pInterface->clientDriver].EventHandler(address, event, data, size);
                    break;
                }
                pEndPoint = pEndPoint->next;
            }
            break;
        default:
            while (pInterface != NULL)  // Scan the interface list for all active drivers.
            {
                usbClientDrvTable[pInterface->clientDriver].EventHandler(address, event, data, size);
                pInterface = pInterface->next;
            }
            break;
        }

    }
} // _USB_NotifyClients


/****************************************************************************
  Function:
    BOOL _USB_ParseConfigurationDescriptor( void )

  Description:
    This function parses all the endpoint descriptors for the required
    setting of the required interface and sets up the internal endpoint
    information.

  Precondition:
    pCurrentConfigurationDescriptor points to a valid Configuration
    Descriptor, which contains the endpoint descriptors.  The current
    interface and the current interface settings must be set up in
    usbDeviceInfo.

  Parameters:
    None - None

  Returns:
    TRUE    - Successful
    FALSE   - Configuration not supported.

  Remarks:
    * This function also automatically resets all endpoints (except
        endpoint 0) to DATA0, so _USB_ResetDATA0 does not have to be
        called.

    * If the configuration is not supported, the caller will need to clean
        up, freeing memory by calling _USB_FreeConfigMemory.

    * We do not currently implement checks for descriptors that are shorter
    	than the expected length, in the case of invalid USB Peripherals.

    * If there is not enough available heap space for storing the
     	interface or endpoint information, this function will return FALSE.
     	Currently, there is no other mechanism for informing the user of
     	an out of dynamic memory condition.

    * We are assuming that we can support a single interface on a single
        device.  When the driver is modified to support multiple devices,
        each endpoint should be checked to ensure that we have enough
        bandwidth to support it.
  ***************************************************************************/

BOOL _USB_ParseConfigurationDescriptor( void )
{
    BYTE                        bAlternateSetting;
    BYTE                        bDescriptorType;
    BYTE                        bInterfaceNumber;
    BYTE                        bLength;
    BYTE                        bNumEndpoints;
    BYTE                        bNumInterfaces;
    BYTE                        bMaxPower;
    BYTE                        Class;
    BYTE                        SubClass;
    BYTE                        Protocol;
    BYTE                        ClientDriver;
    WORD                        wTotalLength;

    BYTE                        currentAlternateSetting;
    BYTE                        currentConfiguration;
    BYTE                        currentEndpoint;
    BYTE                        currentInterface;
    WORD                        index;
    USB_ENDPOINT_INFO           *newEndpointInfo;
    USB_INTERFACE_INFO          *newInterfaceInfo;
    USB_VBUS_POWER_EVENT_DATA   powerRequest;
    BYTE                        *ptr;

    // Prime the loops.
    index                   = 0;
    ptr                     = pCurrentConfigurationDescriptor;
    currentInterface        = 0;
    currentAlternateSetting = 0;

    // Assume no OTG support (determine otherwise, below).
    usbDeviceInfo.flags.bfSupportsOTG   = 0;
    usbDeviceInfo.flags.bfConfiguredOTG = 1;

    #ifdef USB_SUPPORT_OTG
        usbDeviceInfo.flags.bfAllowHNP = 1;  //Allow HNP From Host
    #endif

    // Load up the values from the Configuration Descriptor
    bLength              = *ptr++;
    bDescriptorType      = *ptr++;
    wTotalLength         = *ptr++;           // In case these are not word aligned
    wTotalLength        += (*ptr++) << 8;
    bNumInterfaces       = *ptr++;
    currentConfiguration = *ptr++;  // bConfigurationValue
                            ptr++;  // iConfiguration
                            ptr++;  // bmAttributes
    bMaxPower            = *ptr;

    // Check Max Power to see if we can support this configuration.
    powerRequest.current = bMaxPower;
    powerRequest.port    = 0;        // Port 0
    if (!USB_HOST_APP_EVENT_HANDLER( USB_ROOT_HUB, EVENT_VBUS_REQUEST_POWER,
            &powerRequest, sizeof(USB_VBUS_POWER_EVENT_DATA) ))
    {
        usbDeviceInfo.errorCode = USB_ERROR_INSUFFICIENT_POWER;
        return FALSE;
    }

    // Skip over the rest of the Configuration Descriptor
    index += bLength;
    ptr    = &pCurrentConfigurationDescriptor[index];

    while (index < wTotalLength)
    {
        // Check the descriptor length and type
        bLength         = *ptr++;
        bDescriptorType = *ptr++;


        // Find the OTG discriptor (if present)
        if (bDescriptorType == USB_DESCRIPTOR_OTG)
        {
            // We found an OTG Descriptor, so the device supports OTG.
            usbDeviceInfo.flags.bfSupportsOTG = 1;
            usbDeviceInfo.attributesOTG       = *ptr;

            // See if we need to send the SET FEATURE command.  If we do,
            // clear the bConfiguredOTG flag.
            if ( (usbDeviceInfo.attributesOTG & OTG_HNP_SUPPORT) && (usbDeviceInfo.flags.bfAllowHNP))
            {
                usbDeviceInfo.flags.bfConfiguredOTG = 0;
            }
            else
            {
                usbDeviceInfo.flags.bfAllowHNP = 0;
            }
        }

        // Find the interface descriptor
        if (bDescriptorType != USB_DESCRIPTOR_INTERFACE)
        {
            // Skip over the rest of the Descriptor
            index += bLength;
            ptr = &pCurrentConfigurationDescriptor[index];
        }
        else
        {
            // Read some data from the interface descriptor
            bInterfaceNumber  = *ptr++;
            bAlternateSetting = *ptr++;
            bNumEndpoints     = *ptr++;
            Class             = *ptr++;
            SubClass          = *ptr++;
            Protocol          = *ptr++;

            // Get client driver index
            if (usbDeviceInfo.flags.bfUseDeviceClientDriver)
            {
                ClientDriver = usbDeviceInfo.deviceClientDriver;
            }
            else
            {
                if (!_USB_FindClassDriver(Class, SubClass, Protocol, &ClientDriver))
                {
                    // Skip to the next interface descriptor
                    currentAlternateSetting++;
                    index += bLength;
                    ptr = &pCurrentConfigurationDescriptor[index];
                    continue;
                }
            }

            // See if this is the interface and setting we want.
            if ((currentInterface        != bInterfaceNumber ) ||
                (currentAlternateSetting != bAlternateSetting)   )
            {
                // Skip over the rest of the Descriptor
                index += bLength;
                ptr = &pCurrentConfigurationDescriptor[index];
            }
            else
            {
                // This is the setting we want.  Create an entry for the new interface.
                if ((newInterfaceInfo = (USB_INTERFACE_INFO *)malloc( sizeof(USB_INTERFACE_INFO) )) == NULL)
                {
                    return FALSE;   // Out of memory
                }

                // Initialize the interface node
                newInterfaceInfo->interface           = bInterfaceNumber;
                newInterfaceInfo->interfaceAltSetting = bAlternateSetting;
                newInterfaceInfo->clientDriver        = ClientDriver;

                // Insert it into the list.
                newInterfaceInfo->next                = (USB_INTERFACE_INFO *)pInterfaceList;
                pInterfaceList                        = newInterfaceInfo;

                // Advance to the next interface
                currentInterface++;
                currentAlternateSetting = 0;

                //  Configure the endpoints for this interface.
                currentEndpoint = 0;

                // Skip over the rest of the Interface Descriptor
                index += bLength;
                ptr = &pCurrentConfigurationDescriptor[index];

                // Find the Endpoint Descriptors.  There might be Class and Vendor descriptors in here
                while ((index < wTotalLength) && (currentEndpoint < bNumEndpoints))
                {
                    bLength = *ptr++;
                    bDescriptorType = *ptr++;

                    if (bDescriptorType != USB_DESCRIPTOR_ENDPOINT)
                    {
                        // Skip over the rest of the Descriptor
                        index += bLength;
                        ptr = &pCurrentConfigurationDescriptor[index];
                    }
                    else
                    {
                        // Create an entry for the new endpoint.
                        if ((newEndpointInfo = (USB_ENDPOINT_INFO *)malloc( sizeof(USB_ENDPOINT_INFO) )) == NULL)
                        {
                            return FALSE;   // Out of memory
                        }
                        newEndpointInfo->bEndpointAddress           = *ptr++;
                        newEndpointInfo->bmAttributes.val           = *ptr++;
                        newEndpointInfo->wMaxPacketSize             = *ptr++;
                        newEndpointInfo->wMaxPacketSize            += (*ptr++) << 8;
                        newEndpointInfo->wInterval                  = *ptr++;
                        newEndpointInfo->status.val                 = 0x00;
                        newEndpointInfo->status.bfUseDTS            = 1;
                        newEndpointInfo->status.bfTransferComplete  = 1;  // Initialize to success to allow preprocessing loops.
                        newEndpointInfo->dataCount                  = 0;  // Initialize to 0 since we set bfTransferComplete.
                        newEndpointInfo->transferState              = TSTATE_IDLE;
                        newEndpointInfo->pInterface                 = newInterfaceInfo;

                        // Special setup for isochronous endpoints.
                        if (newEndpointInfo->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_ISOCHRONOUS)
                        {
                            // Validate and convert the interval to the number of frames.  The value must
                            // be between 1 and 16, and the frames is 2^(bInterval-1).
                            if (newEndpointInfo->wInterval == 0) newEndpointInfo->wInterval = 1;
                            if (newEndpointInfo->wInterval > 16) newEndpointInfo->wInterval = 16;
                            newEndpointInfo->wInterval = 1 << (newEndpointInfo->wInterval-1);

                            // Disable DTS
                            newEndpointInfo->status.bfUseDTS = 0;
                        }

                        // Initialize interval count
                        newEndpointInfo->wIntervalCount = newEndpointInfo->wInterval;
                        // Put the new endpoint in the list after EP0.
                        newEndpointInfo->next = pEndpointList->next;
                        pEndpointList->next   = newEndpointInfo;

                        // When multiple devices are supported, check the available
                        // bandwidth here to make sure that we can support this
                        // endpoint.

                        // Get ready for the next endpoint.
                        currentEndpoint++;
                        index += bLength;
                        ptr = &pCurrentConfigurationDescriptor[index];
                    }
                }

                // Ensure that we found all the endpoints for this interface.
                if (currentEndpoint != bNumEndpoints)
                {
                    return FALSE;
                }
            }
        }
    }

    // Ensure that we found all the interfaces in this configuration.
    // This is a nice check, but some devices have errors where they have a
    // different number of interfaces than they report they have!
//    if (currentInterface != bNumInterfaces)
//    {
//        return FALSE;
//    }

    if (pInterfaceList == NULL)
    {
        // We could find no supported interfaces.
        #ifdef DEBUG_MODE
            UART2PrintString( "HOST: No supported interfaces.\r\n" );
        #endif

        return FALSE;
    }

    // Set configuration.
    usbDeviceInfo.currentConfiguration      = currentConfiguration;
    usbDeviceInfo.currentConfigurationPower = bMaxPower;

    // Success!
    #ifdef DEBUG_MODE
        UART2PrintString( "HOST: Parse Descriptor success\r\n" );
    #endif
    return TRUE;
}


/****************************************************************************
  Function:
    void _USB_ResetDATA0( BYTE endpoint )

  Description:
    This function resets DATA0 for the specified endpoint.  If the
    specified endpoint is 0, it resets DATA0 for all endpoints.

  Precondition:
    None

  Parameters:
    BYTE endpoint   - Endpoint number to reset.


  Returns:
    None

  Remarks:
    None
  ***************************************************************************/

void _USB_ResetDATA0( BYTE endpoint )
{
    USB_ENDPOINT_INFO   *ep;

    ep = pEndpointList;
    while (ep != NULL)
    {
        if ((endpoint == 0) || (endpoint == ep->bEndpointAddress))
        {
            ep->status.bfNextDATA01 = 0;
        }
        ep = ep->next;
    }
}


/****************************************************************************
  Function:
    void _USB_SendToken( BYTE endpoint, BYTE tokenType )

  Description:
    This function sets up the endpoint control register and sends the token.

  Precondition:
    None

  Parameters:
    BYTE endpoint   - Endpoint number
    BYTE tokenType  - Token to send

  Returns:
    None

  Remarks:
    If the device is low speed, the transfer must be set to low speed.  If
    the endpoint is isochronous, handshaking must be disabled.
  ***************************************************************************/

void _USB_SendToken( BYTE endpoint, BYTE tokenType )
{
    BYTE    temp;

    // Disable retries, disable control transfers, enable Rx and Tx and handshaking.
    temp = 0x5D;

    // Enable low speed transfer if the device is low speed.
    if (usbDeviceInfo.flags.bfIsLowSpeed)
    {
        temp |= 0x80;   // Set LSPD
    }

    // Enable control transfers if necessary.
    if (pCurrentEndpoint->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_CONTROL)
    {
        temp &= 0xEF;   // Clear EPCONDIS
    }

    // Disable handshaking for isochronous endpoints.
    if (pCurrentEndpoint->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_ISOCHRONOUS)
    {
        temp &= 0xFE;   // Clear EPHSHK
    }

    U1EP0 = temp;

    #ifdef DEBUG_MODE
        if (usbBusInfo.flags.bfTokenAlreadyWritten) UART2PutChar( '+' );
//        if (U1CONbits.TOKBUSY) UART2PutChar( '+' );
    #endif

    U1ADDR = usbDeviceInfo.deviceAddressAndSpeed;
    U1TOK = (tokenType << 4) | (endpoint & 0x7F);

    // Lock out anyone from writing another token until this one has finished.
//    U1CONbits.TOKBUSY = 1;
    usbBusInfo.flags.bfTokenAlreadyWritten = 1;

    #ifdef DEBUG_MODE
        //UART2PutChar('(');
        //UART2PutHex(U1ADDR);
        //UART2PutHex(U1EP0);
        //UART2PutHex(U1TOK);
        //UART2PutChar(')');
    #endif
}


/****************************************************************************
  Function:
    void _USB_SetBDT( BYTE token )

  Description:
    This function sets up the BDT for the transfer.  The function handles the
    different ping-pong modes.

  Precondition:
    pCurrentEndpoint must point to the current endpoint being serviced.

  Parameters:
    BYTE token  - Token for the transfer.  That way we can tell which
                    ping-pong buffer and which data pointer to use.  Valid
                    values are:
                        * USB_TOKEN_SETUP
                        * USB_TOKEN_IN
                        * USB_TOKEN_OUT

  Returns:
    None

  Remarks:
    None
  ***************************************************************************/

void _USB_SetBDT( BYTE token )
{
    WORD                currentPacketSize;
    BDT_ENTRY           *pBDT;

    if (token == USB_TOKEN_IN)
    {
        // Find the BDT we need to use.
        #if (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG)
            pBDT = BDT_IN;
            if (usbDeviceInfo.flags.bfPingPongIn)
            {
                pBDT = BDT_IN_ODD;
            }
        #else
            pBDT = BDT_IN;
        #endif

        // Set up ping-pong for the next transfer
        #if (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG)
            usbDeviceInfo.flags.bfPingPongIn = ~usbDeviceInfo.flags.bfPingPongIn;
        #endif
    }
    else  // USB_TOKEN_OUT or USB_TOKEN_SETUP
    {
        // Find the BDT we need to use.
        #if (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG) || (USB_PING_PONG_MODE == USB_PING_PONG__EP0_OUT_ONLY)
            pBDT = BDT_OUT;
            if (usbDeviceInfo.flags.bfPingPongOut)
            {
                pBDT = BDT_OUT_ODD;
            }
        #else
            pBDT = BDT_OUT;
        #endif

        // Set up ping-pong for the next transfer
        #if (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG) || (USB_PING_PONG_MODE == USB_PING_PONG__EP0_OUT_ONLY)
            usbDeviceInfo.flags.bfPingPongOut = ~usbDeviceInfo.flags.bfPingPongOut;
        #endif
    }

    // Determine how much data we'll transfer in this packet.
    if (token == USB_TOKEN_SETUP)
    {
        if ((pCurrentEndpoint->dataCountMaxSETUP - pCurrentEndpoint->dataCount) > pCurrentEndpoint->wMaxPacketSize)
        {
            currentPacketSize = pCurrentEndpoint->wMaxPacketSize;
        }
        else
        {
            currentPacketSize = pCurrentEndpoint->dataCountMaxSETUP - pCurrentEndpoint->dataCount;
        }
    }
    else
    {
        if ((pCurrentEndpoint->dataCountMax - pCurrentEndpoint->dataCount) > pCurrentEndpoint->wMaxPacketSize)
        {
            currentPacketSize = pCurrentEndpoint->wMaxPacketSize;
        }
        else
        {
            currentPacketSize = pCurrentEndpoint->dataCountMax - pCurrentEndpoint->dataCount;
        }
    }

    // Load up the BDT address.
    if (token == USB_TOKEN_SETUP)
    {
        #if defined(__C30__)
            pBDT->ADR  = pCurrentEndpoint->pUserDataSETUP;
        #elif defined(__PIC32MX__)
            pBDT->ADR  = (BYTE *)KVA_TO_PA((DWORD)pCurrentEndpoint->pUserDataSETUP);
        #else
            #error Cannot set BDT address.
        #endif
    }
    else
    {
        #if defined(__C30__)
            pBDT->ADR  = (BYTE *)((WORD)pCurrentEndpoint->pUserData + (WORD)pCurrentEndpoint->dataCount);
        #elif defined(__PIC32MX__)
            pBDT->ADR  = (BYTE *)KVA_TO_PA((DWORD)pCurrentEndpoint->pUserData + (DWORD)pCurrentEndpoint->dataCount);
        #else
            #error Cannot set BDT address.
        #endif
    }

    // Load up the BDT status register.
    pBDT->STAT.Val      = 0;
    pBDT->count         = currentPacketSize;
    pBDT->STAT.DTS      = pCurrentEndpoint->status.bfNextDATA01;
    pBDT->STAT.DTSEN    = pCurrentEndpoint->status.bfUseDTS;

    // Transfer the BD to the USB OTG module.
    pBDT->STAT.UOWN     = 1;

    #ifdef DEBUG_MODE
//        UART2PutChar('{');
//        UART2PutHex((pBDT->v[0] >> 24) & 0xff);
//        UART2PutHex((pBDT->v[0] >> 16) & 0xff);
//        UART2PutHex((pBDT->v[0] >> 8) & 0xff);
//        UART2PutHex((pBDT->v[0]) & 0xff);
//        UART2PutChar('-');
//        UART2PutHex((currentPacketSize >> 24) & 0xff);
//        UART2PutHex((pBDT->v[1] >> 16) & 0xff);
//        UART2PutHex((currentPacketSize >> 8) & 0xff);
//        UART2PutHex(currentPacketSize & 0xff);
//        UART2PutChar('}');
    #endif

}


/****************************************************************************
  Function:
    BOOL _USB_TransferInProgress( void )

  Description:
    This function checks to see if any read or write transfers are in
    progress.

  Precondition:
    None

  Parameters:
    None - None

  Returns:
    TRUE    - At least one read or write transfer is occurring.
    FALSE   - No read or write transfers are occurring.

  Remarks:
    None
  ***************************************************************************/

BOOL _USB_TransferInProgress( void )
{
    USB_ENDPOINT_INFO   *ep;

    ep = pEndpointList;
    while (ep != NULL)
    {
        if (!ep->status.bfTransferComplete)
        {
            return TRUE;
        }
        ep = ep->next;
    }
    return FALSE;
}


// *****************************************************************************
// *****************************************************************************
// Section: Interrupt Handlers
// *****************************************************************************
// *****************************************************************************

/****************************************************************************
  Function:
    void _USB1Interrupt( void )

  Summary:
    This is the interrupt service routine for the USB interrupt.

  Description:
    This is the interrupt service routine for the USB interrupt.  The
    following cases are serviced:
         * Device Attach
         * Device Detach
         * One millisecond Timer
         * Start of Frame
         * Transfer Done
         * USB Error

  Precondition:
    In TRNIF handling, pCurrentEndpoint is still pointing to the last
    endpoint to which a token was sent.

  Parameters:
    None - None

  Returns:
    None

  Remarks:
    None
  ***************************************************************************/
#define U1STAT_TX_MASK                      0x08    // U1STAT bit mask for Tx/Rx indication
#define U1STAT_ODD_MASK                     0x04    // U1STAT bit mask for even/odd buffer bank

#if defined(__C30__)
void __attribute__((__interrupt__, auto_psv)) _USB1Interrupt( void )
#elif defined(__PIC32MX__)
#pragma interrupt _USB1Interrupt ipl4 vector 45
void _USB1Interrupt( void )
#else
    #error Cannot define timer interrupt vector.
#endif
{
    #if defined( __C30__)
        IFS5 &= 0xFFBF;
    #elif defined( __PIC32MX__)
        IFS1CLR = 0x02000000;
    #else
        #error Cannot clear USB interrupt.
    #endif

    // -------------------------------------------------------------------------
    // One Millisecond Timer ISR

    if (U1OTGIEbits.T1MSECIE && U1OTGIRbits.T1MSECIF)
    {
        // The interrupt is cleared by writing a '1' to it.
        U1OTGIR = USB_INTERRUPT_T1MSECIF;

        #ifdef DEBUG_MODE
            UART2PutChar('~');
        #endif

        #ifdef  USB_SUPPORT_OTG
            if (USBOTGGetSRPTimeOutFlag())
            {
                if (USBOTGIsSRPTimeOutExpired())
                {
                    USB_OTGEventHandler(0,OTG_EVENT_SRP_FAILED,0,0);
                }

            }

            else if (USBOTGGetHNPTimeOutFlag())
            {
                if (USBOTGIsHNPTimeOutExpired())
                {
                    USB_OTGEventHandler(0,OTG_EVENT_HNP_FAILED,0,0);
                }

            }

            else
            {
                numTimerInterrupts--;
                if (numTimerInterrupts == 0)
                {
                    // Turn off the timer interrupt.
                    U1OTGIEbits.T1MSECIE = 0;

                    // Advance to the next state.  We can do this here, because the only time
                    // we'll get a time interrupt is while we are in one of the holding states.
                    _USB_SetNextSubSubState();
                }
            }
         #else

            numTimerInterrupts--;
            if (numTimerInterrupts == 0)
            {
                // Turn off the timer interrupt.
                U1OTGIEbits.T1MSECIE = 0;

                // Advance to the next state.  We can do this here, because the only time
                // we'll get a time interrupt is while we are in one of the holding states.
                _USB_SetNextSubSubState();
            }
         #endif
    }

    // -------------------------------------------------------------------------
    // Attach ISR

    // The attach interrupt is level, not edge, triggered.  So make sure we have it enabled.
    if (U1IEbits.ATTACHIE && U1IRbits.ATTACHIF)
    {
        #ifdef DEBUG_MODE
            UART2PutChar( '[' );
        #endif

        // The attach interrupt is level, not edge, triggered.  If we clear it, it just
        // comes right back.  So clear the enable instead
        U1IEbits.ATTACHIE   = 0;
        U1IR                = USB_INTERRUPT_ATTACH;

        if (usbHostState == (STATE_DETACHED | SUBSTATE_WAIT_FOR_DEVICE))
        {
            _USB_SetNextState();
        }


        #ifdef  USB_SUPPORT_OTG
            //If HNP Related Attach, Process Connect Event
            USB_OTGEventHandler(0, OTG_EVENT_CONNECT, 0, 0 );

            //If SRP Related A side D+ High, Process D+ High Event
            USB_OTGEventHandler (0, OTG_EVENT_SRP_DPLUS_HIGH, 0, 0 );

            //If SRP Related B side Attach
            USB_OTGEventHandler (0, OTG_EVENT_SRP_CONNECT, 0, 0 );
        #endif

    }

    // -------------------------------------------------------------------------
    // Detach ISR

    if (U1IEbits.DETACHIE && U1IRbits.DETACHIF)
    {
        #ifdef DEBUG_MODE
            UART2PutChar( ']' );
        #endif

        U1IR                    = USB_INTERRUPT_DETACH;
        U1IEbits.DETACHIE       = 0;
        usbOverrideHostState    = STATE_DETACHED;

        #ifdef  USB_SUPPORT_OTG
            //If HNP Related Detach Detected, Process Disconnect Event
            USB_OTGEventHandler (0, OTG_EVENT_DISCONNECT, 0, 0 );

            //If SRP Related D+ Low and SRP Is Active, Process D+ Low Event
            USB_OTGEventHandler (0, OTG_EVENT_SRP_DPLUS_LOW, 0, 0 );

            //Disable HNP, Detach Interrupt Could've Triggered From Cable Being Unplugged
            USBOTGDisableHnp();
        #endif
    }

    #ifdef USB_SUPPORT_OTG

        // -------------------------------------------------------------------------
        //ID Pin Change ISR
        if (U1OTGIRbits.IDIF && U1OTGIEbits.IDIE)
        {
             USBOTGInitialize();

             //Clear Interrupt Flag
             U1OTGIR = 0x80;
        }

        // -------------------------------------------------------------------------
        //VB_SESS_END ISR
        if (U1OTGIRbits.SESENDIF && U1OTGIEbits.SESENDIE)
        {
            //If B side Host And Cable Was Detached Then
            if (U1OTGSTATbits.ID == CABLE_B_SIDE && USBOTGCurrentRoleIs() == ROLE_HOST)
            {
                //Reinitialize
                USBOTGInitialize();
            }

            //Clear Interrupt Flag
            U1OTGIR = 0x04;
        }

        // -------------------------------------------------------------------------
        //VA_SESS_VLD ISR
        if (U1OTGIRbits.SESVDIF && U1OTGIEbits.SESVDIE)
        {
            //If A side Host and SRP Is Active Then
            if (USBOTGDefaultRoleIs() == ROLE_HOST && USBOTGSrpIsActive())
            {
                //If VBUS > VA_SESS_VLD Then
                if (U1OTGSTATbits.SESVD == 1)
                {
                    //Process SRP VBUS High Event
                    USB_OTGEventHandler (0, OTG_EVENT_SRP_VBUS_HIGH, 0, 0 );
                }

                //If VBUS < VA_SESS_VLD Then
                else
                {
                     //Process SRP Low Event
                    USB_OTGEventHandler (0, OTG_EVENT_SRP_VBUS_LOW, 0, 0 );
                }
            }

            U1OTGIR = 0x08;
        }

        // -------------------------------------------------------------------------
        //Resume Signaling for Remote Wakeup
        if (U1IRbits.RESUMEIF && U1IEbits.RESUMEIE)
        {
            //Process SRP VBUS High Event
            USB_OTGEventHandler (0, OTG_EVENT_RESUME_SIGNALING,0, 0 );

            //Clear Resume Interrupt Flag
            U1IR = 0x20;
        }
    #endif


    // -------------------------------------------------------------------------
    // Transfer Done ISR - only process if there was no error

    if ((U1IEbits.TRNIE && U1IRbits.TRNIF) && !(U1IEbits.UERRIE && U1IRbits.UERRIF))
    {
        #if defined(__C30__)
            U1STATBITS          copyU1STATbits;
        #elif defined(__PIC32MX__)
            __U1STATbits_t      copyU1STATbits;
        #else
            #error Need structure name for copyU1STATbits.
        #endif
        WORD                    packetSize;
        BDT_ENTRY               *pBDT;

        #ifdef DEBUG_MODE
            UART2PutChar( '!' );
        #endif

        // The previous token has finished, so clear the way for writing a new one.
        usbBusInfo.flags.bfTokenAlreadyWritten = 0;

        copyU1STATbits = U1STATbits;    // Read the status register before clearing the flag.

        U1IR = USB_INTERRUPT_TRANSFER;  // Clear the interrupt by writing a '1' to the flag.

        // In host mode, U1STAT does NOT reflect the endpoint.  It is really the last updated
        // BDT, which, in host mode, is always 0.  To get the endpoint, we either need to look
        // at U1TOK, or trust that pCurrentEndpoint is still accurate.
        if ((pCurrentEndpoint->bEndpointAddress & 0x0F) == (U1TOK & 0x0F))
        {
            if (copyU1STATbits.DIR)     // TX
            {
                // We are processing OUT or SETUP packets.
                // Set up the BDT pointer for the transaction we just received.
                #if (USB_PING_PONG_MODE == USB_PING_PONG__EP0_OUT_ONLY) || (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG)
                    pBDT = BDT_OUT;
                    if (copyU1STATbits.PPBI) // Odd
                    {
                        pBDT = BDT_OUT_ODD;
                    }
                #elif (USB_PING_PONG_MODE == USB_PING_PONG__NO_PING_PONG) || (USB_PING_PONG_MODE == USB_PING_PONG__ALL_BUT_EP0)
                    pBDT = BDT_OUT;
                #endif
            }
            else
            {
                // We are processing IN packets.
                // Set up the BDT pointer for the transaction we just received.
                #if (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG)
                    pBDT = BDT_IN;
                    if (copyU1STATbits.PPBI) // Odd
                    {
                        pBDT = BDT_IN_ODD;
                    }
                #else
                    pBDT = BDT_IN;
                #endif
            }

            if (pBDT->STAT.PID == PID_ACK)
            {
                // We will only get this PID from an OUT or SETUP packet.

                // Update the count of bytes tranferred.  (If there was an error, this count will be 0.)
                // The Byte Count is NOT 0 if a NAK occurs.  Therefore, we can only update the
                // count when an ACK, DATA0, or DATA1 is received.
                packetSize                  = pBDT->count;
                pCurrentEndpoint->dataCount += packetSize;

                // Set the NAK retries for the next transaction;
                pCurrentEndpoint->countNAKs = 0;

                // Toggle DTS for the next transfer.
                pCurrentEndpoint->status.bfNextDATA01 ^= 0x01;

                if ((pCurrentEndpoint->transferState == (TSTATE_CONTROL_NO_DATA | TSUBSTATE_CONTROL_NO_DATA_SETUP)) ||
                    (pCurrentEndpoint->transferState == (TSTATE_CONTROL_READ    | TSUBSTATE_CONTROL_READ_SETUP)) ||
                    (pCurrentEndpoint->transferState == (TSTATE_CONTROL_WRITE   | TSUBSTATE_CONTROL_WRITE_SETUP)))
                {
                    // We are doing SETUP transfers. See if we are done with the SETUP portion.
                    if (pCurrentEndpoint->dataCount >= pCurrentEndpoint->dataCountMaxSETUP)
                    {
                        // We are done with the SETUP.  Reset the byte count and
                        // proceed to the next token.
                        pCurrentEndpoint->dataCount = 0;
                        _USB_SetNextTransferState();
                    }
                }
                else
                {
                    // We are doing OUT transfers.  See if we've written all the data.
                    // We've written all the data when we send a short packet or we have
                    // transferred all the data.
                    if ((packetSize < pCurrentEndpoint->wMaxPacketSize) ||
                        (pCurrentEndpoint->dataCount >= pCurrentEndpoint->dataCountMax))
                    {
                        // We've written all the data. Proceed to the next step.
                        pCurrentEndpoint->status.bfTransferSuccessful = 1;
                        _USB_SetNextTransferState();
                    }
                    else
                    {
                        // We need to process more data.  Keep this endpoint in its current
                        // transfer state.
                    }
                }
            }
            else if ((pBDT->STAT.PID == PID_DATA0) || (pBDT->STAT.PID == PID_DATA1))
            {
                // We will only get these PID's from an IN packet.

                // Update the count of bytes tranferred.  (If there was an error, this count will be 0.)
                // The Byte Count is NOT 0 if a NAK occurs.  Therefore, we can only update the
                // count when an ACK, DATA0, or DATA1 is received.
                packetSize                  = pBDT->count;
                pCurrentEndpoint->dataCount += packetSize;

                // Set the NAK retries for the next transaction;
                pCurrentEndpoint->countNAKs = 0;

                // Toggle DTS for the next transfer.
                if (pCurrentEndpoint->status.bfNextDATA01)
                {
                    pCurrentEndpoint->status.bfNextDATA01 = 0;
                }
                else
                {
                    pCurrentEndpoint->status.bfNextDATA01 = 1;
                }

                // We are doing IN transfers.  See if we've received all the data.
                // We've received all the data when we receive a short packet or we have
                // transferred all the data.
                if ((packetSize < pCurrentEndpoint->wMaxPacketSize) ||
                    (pCurrentEndpoint->dataCount >= pCurrentEndpoint->dataCountMax))
                {
                    // If we've received all the data, stop the transfer.  We've received all the
                    // data when we receive a short or zero-length packet.  If the data length is a
                    // multiple of wMaxPacketSize, we will get a 0-length packet.
                    pCurrentEndpoint->status.bfTransferSuccessful = 1;
                    _USB_SetNextTransferState();
                }
                else
                {
                    // We need to process more data.  Keep this endpoint in its current
                    // transfer state.
                }
            }
            else if (pBDT->STAT.PID == PID_NAK)
            {
                #ifndef ALLOW_MULTIPLE_NAKS_PER_FRAME
                    pCurrentEndpoint->status.bfLastTransferNAKd = 1;
                #endif

                pCurrentEndpoint->countNAKs ++;

                switch( pCurrentEndpoint->bmAttributes.bfTransferType )
                {
                    case USB_TRANSFER_TYPE_BULK:
                        // Bulk IN and OUT transfers are allowed to retry NAK'd
                        // transactions until a timeout (if enabled) or indefinitely
                            // (if NAK timeouts disabled).
                        if (pCurrentEndpoint->status.bfNAKTimeoutEnabled &&
                            (pCurrentEndpoint->countNAKs > pCurrentEndpoint->timeoutNAKs))
                        {
                            pCurrentEndpoint->status.bfError    = 1;
                            pCurrentEndpoint->bErrorCode        = USB_ENDPOINT_NAK_TIMEOUT;
                            _USB_SetTransferErrorState( pCurrentEndpoint );
                        }
                        break;

                    case USB_TRANSFER_TYPE_CONTROL:
                        // Devices should not NAK the SETUP portion.  If they NAK
                        // the DATA portion, they are allowed to retry a fixed
                        // number of times.
                        if (pCurrentEndpoint->status.bfNAKTimeoutEnabled &&
                            (pCurrentEndpoint->countNAKs > pCurrentEndpoint->timeoutNAKs))
                        {
                            pCurrentEndpoint->status.bfError    = 1;
                            pCurrentEndpoint->bErrorCode        = USB_ENDPOINT_NAK_TIMEOUT;
                            _USB_SetTransferErrorState( pCurrentEndpoint );
                        }
                        break;

                    case USB_TRANSFER_TYPE_INTERRUPT:
                        if ((pCurrentEndpoint->bEndpointAddress & 0x80) == 0x00)
                        {
                            // Interrupt OUT transfers are allowed to retry NAK'd
                            // transactions until a timeout (if enabled) or indefinitely
                            // (if NAK timeouts disabled).
                            if (pCurrentEndpoint->status.bfNAKTimeoutEnabled &&
                                (pCurrentEndpoint->countNAKs > pCurrentEndpoint->timeoutNAKs))
                            {
                                pCurrentEndpoint->status.bfError    = 1;
                                pCurrentEndpoint->bErrorCode        = USB_ENDPOINT_NAK_TIMEOUT;
                                _USB_SetTransferErrorState( pCurrentEndpoint );
                            }
                        }
                        else
                        {
                            // Interrupt IN transfers terminate with no error.
                            pCurrentEndpoint->status.bfTransferSuccessful = 1;
                            _USB_SetNextTransferState();
                        }
                        break;

                    case USB_TRANSFER_TYPE_ISOCHRONOUS:
                        // Isochronous transfers terminate with no error.
                        pCurrentEndpoint->status.bfTransferSuccessful = 1;
                        _USB_SetNextTransferState();
                        break;
                }
            }
            else if (pBDT->STAT.PID == PID_STALL)
            {
                // Device is stalled.  Stop the transfer, and indicate the stall.
                // The application must clear this if not a control endpoint.
                // A stall on a control endpoint does not indicate that the
                // endpoint is halted.
                #ifdef DEBUG_MODE
                    UART2PutChar( '^' );
                #endif
                pCurrentEndpoint->status.bfStalled = 1;
                pCurrentEndpoint->bErrorCode       = USB_ENDPOINT_STALLED;
                _USB_SetTransferErrorState( pCurrentEndpoint );
            }
            else
            {
                // Module-defined PID - Bus Timeout (0x0) or Data Error (0x0F).  Increment the error count.
                pCurrentEndpoint->status.bfErrorCount++;

                if (pCurrentEndpoint->status.bfErrorCount >= 3)
                {
                    // We have too many errors.  Stall the endpoint.
                    pCurrentEndpoint->status.bfStalled  = 1;

                    // Stop the transfer and indicate an error.
                    // The application must clear this.
                    pCurrentEndpoint->status.bfError    = 1;
                    pCurrentEndpoint->bErrorCode        = USB_ENDPOINT_ERROR_ILLEGAL_PID;
                    _USB_SetTransferErrorState( pCurrentEndpoint );

                    // Avoid the error interrupt code, because we are going to
                    // find another token to send.
                    U1EIR = 0xFF;
                    U1IR  = USB_INTERRUPT_ERROR;
                }
                else
                {
                    // Fall through.  This will automatically cause the transfer
                    // to be retried.
                }
            }
        }
        else
        {
            // We have a mismatch between the endpoint we were expecting and the one that we got.
            // The user may be trying to select a new configuration.  Discard the transaction.
        }

        _USB_FindNextToken();
    } // U1IRbits.TRNIF


    // -------------------------------------------------------------------------
    // Start-of-Frame ISR

    if (U1IEbits.SOFIE && U1IRbits.SOFIF)
    {
        USB_ENDPOINT_INFO   *ep;

        #ifdef DEBUG_MODE
//            UART2PutChar( '$' );
        #endif
        U1IR = USB_INTERRUPT_SOF; // Clear the interrupt by writing a '1' to the flag.

        ep = pEndpointList->next;
        while (ep != NULL)
        {
            // Decrement the interval count of all interrupt and isochronous endpoints.
            if ((ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_INTERRUPT) ||
                (ep->bmAttributes.bfTransferType == USB_TRANSFER_TYPE_ISOCHRONOUS))
            {
                if (ep->wIntervalCount != 0)
                {
                    ep->wIntervalCount--;
                }
            }
            #ifndef ALLOW_MULTIPLE_NAKS_PER_FRAME
                ep->status.bfLastTransferNAKd = 0;
            #endif
            ep = ep->next;
        }

        usbBusInfo.flags.bfControlTransfersDone     = 0;
        usbBusInfo.flags.bfInterruptTransfersDone   = 0;
        usbBusInfo.flags.bfIsochronousTransfersDone = 0;
        usbBusInfo.flags.bfBulkTransfersDone        = 0;
        usbBusInfo.dBytesSentInFrame                = 0;
        usbBusInfo.lastBulkTransaction              = 0;

        _USB_FindNextToken();
    }

    // -------------------------------------------------------------------------
    // USB Error ISR

    if (U1IEbits.UERRIE && U1IRbits.UERRIF)
    {
        #ifdef DEBUG_MODE
            UART2PutChar('#');
            UART2PutHex( U1EIR );
        #endif

        // The previous token has finished, so clear the way for writing a new one.
        usbBusInfo.flags.bfTokenAlreadyWritten = 0;

        // Interrupt error.  Increment the error count.
        pCurrentEndpoint->status.bfErrorCount++;

        if (pCurrentEndpoint->status.bfErrorCount >= 3)
        {
            // We have too many errors.  Stall the endpoint.
            pCurrentEndpoint->status.bfStalled  = 1;

            // Check U1EIR for the appropriate error codes to return
            if (U1EIRbits.BTSEF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_BIT_STUFF;
            if (U1EIRbits.DMAEF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_DMA;
            if (U1EIRbits.BTOEF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_TIMEOUT;
            if (U1EIRbits.DFN8EF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_DATA_FIELD;
            if (U1EIRbits.CRC16EF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_CRC16;
            if (U1EIRbits.EOFEF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_END_OF_FRAME;
            if (U1EIRbits.PIDEF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_PID_CHECK;
            #if defined(__PIC32MX__)
            if (U1EIRbits.BMXEF)
                pCurrentEndpoint->bErrorCode = USB_ENDPOINT_ERROR_BMX;
            #endif

            pCurrentEndpoint->status.bfError    = 1;

            _USB_SetTransferErrorState( pCurrentEndpoint );
        }

        U1EIR = 0xFF;   // Clear the interrupts by writing '1' to the flags.
        U1IR = USB_INTERRUPT_ERROR; // Clear the interrupt by writing a '1' to the flag.
    }
}


/*************************************************************************
 * EOF usb_host.c
 */

