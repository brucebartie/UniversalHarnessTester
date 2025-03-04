/********************************************************************
  File Information:
    FileName:     	usb_function_hid.c
    Dependencies:   See INCLUDES section
    Processor:      PIC18 or PIC24 USB Microcontrollers
    Hardware:       The code is natively intended to be used on the following
    				hardware platforms: PICDEM� FS USB Demo Board, 
    				PIC18F85J50 FS USB Plug-In Module, or
    				Explorer 16 + PIC24 USB PIM.  The firmware may be
    				modified for use on other USB platforms by editing the
    				HardwareProfile.h file.
    Complier:  	    Microchip C18 (for PIC18) or C30 (for PIC24)
    Company:        Microchip Technology, Inc.
    
    Software License Agreement:
    
    The software supplied herewith by Microchip Technology Incorporated
    (the �Company�) for its PIC� Microcontroller is intended and
    supplied to you, the Company�s customer, for use solely and
    exclusively on Microchip PIC Microcontroller products. The
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

  File Description:
    
    Change History:
     Rev   Date         Description
     1.0   11/19/2004   Initial release
     2.1   02/26/2007   Updated for simplicity and to use common
                        coding style

  Summary:
    This file contains all of functions, macros, definitions, variables,
    datatypes, etc. that are required for usage with the HID function
    driver. This file should be included in projects that use the HID
    \function driver.
    
    
    
    This file is located in the "\<Install Directory\>\\Microchip\\USB\\HID
    Device Driver" directory.
  Description:
    USB HID Function Driver File
    
    This file contains all of functions, macros, definitions, variables,
    datatypes, etc. that are required for usage with the HID function
    driver. This file should be included in projects that use the HID
    \function driver.
    
    This file is located in the "\<Install Directory\>\\Microchip\\USB\\HID
    Device Driver" directory.
    
    When including this file in a new project, this file can either be
    referenced from the directory in which it was installed or copied
    directly into the user application folder. If the first method is
    chosen to keep the file located in the folder in which it is installed
    then include paths need to be added so that the library and the
    application both know where to reference each others files. If the
    application folder is located in the same folder as the Microchip
    folder (like the current demo folders), then the following include
    paths need to be added to the application's project:
    
    ..\\Include
    
    ..\\..\\Include
    
    ..\\..\\Microchip\\Include
    
    ..\\..\\\<Application Folder\>
    
    ..\\..\\..\\\<Application Folder\>
    
    If a different directory structure is used, modify the paths as
    required. An example using absolute paths instead of relative paths
    would be the following:
    
    C:\\Microchip Solutions\\Microchip\\Include
    
    C:\\Microchip Solutions\\My Demo Application  
*******************************************************************/

/** INCLUDES *******************************************************/
#include "GenericTypeDefs.h"
#include "Compiler.h"
#include "usb_config.h"
#include "usb_device.h"
#include "usb_function_hid.h"

/** VARIABLES ******************************************************/
#pragma udata
BYTE idle_rate;
BYTE active_protocol;   // [0] Boot Protocol [1] Report Protocol
BYTE hid_rpt_rx_len;

/** PRIVATE PROTOTYPES *********************************************/
void HIDGetReportHandler(void);
void HIDSetReportHandler(void);

/** Section: DECLARATIONS ***************************************************/
#pragma code

/** Section: CLASS SPECIFIC REQUESTS ****************************************/

/********************************************************************
	Function:
		void USBCheckHIDRequest(void)
		
 	Summary:
 		This routine handles HID specific request that happen on EP0.  
        This function should be called from the USBCBCheckOtherReq() call back 
        function whenever implementing a HID device.

 	Description:
 		This routine handles HID specific request that happen on EP0.  These
        include, but are not limited to, requests for the HID report 
        descriptors.  This function should be called from the 
        USBCBCheckOtherReq() call back function whenever using an HID device.	

        Typical Usage:
        <code>
        void USBCBCheckOtherReq(void)
        {
            //Since the stack didn't handle the request I need to check
            //  my class drivers to see if it is for them
            USBCheckHIDRequest();
        }
        </code>
		
	PreCondition:
		None
		
	Parameters:
		None
		
	Return Values:
		None
		
	Remarks:
		None
 
 *******************************************************************/
void USBCheckHIDRequest(void)
{
    if(SetupPkt.Recipient != RCPT_INTF) return;
    if(SetupPkt.bIntfID != HID_INTF_ID) return;
    
    /*
     * There are two standard requests that hid.c may support.
     * 1. GET_DSC(DSC_HID,DSC_RPT,DSC_PHY);
     * 2. SET_DSC(DSC_HID,DSC_RPT,DSC_PHY);
     */
    if(SetupPkt.bRequest == GET_DSC)
    {
        switch(SetupPkt.bDescriptorType)
        {
            case DSC_HID:           
                if(USBActiveConfiguration == 1)
                {
                    USBEP0SendROMPtr(
                        (ROM BYTE*)&configDescriptor1 + 18,
                        sizeof(USB_HID_DSC)+3,
                        USB_EP0_INCLUDE_ZERO);
                }
                break;
            case DSC_RPT:             
                if(USBActiveConfiguration == 1)
                {
                    USBEP0SendROMPtr(
                        (ROM BYTE*)&hid_rpt01,
                        sizeof(hid_rpt01),     //See usbcfg.h
                        USB_EP0_INCLUDE_ZERO);
                }
                break;
            case DSC_PHY:
                USBEP0Transmit(USB_EP0_NO_DATA);
                break;
        }//end switch(SetupPkt.bDescriptorType)
    }//end if(SetupPkt.bRequest == GET_DSC)
    
    if(SetupPkt.RequestType != CLASS) return;
    switch(SetupPkt.bRequest)
    {
        case GET_REPORT:
            HIDGetReportHandler();
            break;
        case SET_REPORT:
            HIDSetReportHandler();            
            break;
        case GET_IDLE:
            USBEP0SendRAMPtr(
                (BYTE*)&idle_rate,
                1,
                USB_EP0_INCLUDE_ZERO);
            break;
        case SET_IDLE:
            USBEP0Transmit(USB_EP0_NO_DATA);
            idle_rate = SetupPkt.W_Value.byte.HB;
            break;
        case GET_PROTOCOL:
            USBEP0SendRAMPtr(
                (BYTE*)&active_protocol,
                1,
                USB_EP0_NO_OPTIONS);
            break;
        case SET_PROTOCOL:
            USBEP0Transmit(USB_EP0_NO_DATA);
            active_protocol = SetupPkt.W_Value.byte.LB;
            break;
    }//end switch(SetupPkt.bRequest)

}//end USBCheckHIDRequest

void HIDGetReportHandler(void)
{
}//end HIDGetReportHandler

void HIDSetReportHandler(void)
{
}//end HIDSetReportHandler

/** USER API *******************************************************/

/** EOF usb_function_hid.c ******************************************************/
