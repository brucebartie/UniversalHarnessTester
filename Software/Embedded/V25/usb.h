
#ifndef _USB_H_
#define _USB_H_

#include "usb_config.h"

#include "usb_common.h"         // Common USB library definitions
#include "usb_ch9.h"            // USB device framework definitions

#if defined( USB_SUPPORT_DEVICE )
    #include "usb_device.h"     // USB Device abstraction layer interface
#endif

#if defined( USB_SUPPORT_HOST )
    #include "usb_host.h"       // USB Host abstraction layer interface
#endif

#include "usb_hal.h"            // Hardware Abstraction Layer interface

/* USB Library version number.  This can be used to verify in an application
   specific version of the library is being used.
 */
#define USB_MAJOR_VER   2        // Firmware version, major release number.
#define USB_MINOR_VER   18       // Firmware version, minor release number.
#define USB_DOT_VER     0        // Firmware version, dot release number.

#endif // _USB_H_



