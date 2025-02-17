#include <xc.h>
#include "HardwareProfile.h"
#include "system.h"
#include "usb.h"
#include "usb_device_hid.h"
#include "app_device_custom_hid.h"
#include "init.h"
#include "main.h"

void SYSTEM_Initialize( unsigned char state )
{
    switch(state)
    {
        case SYSTEM_STATE_USB_START:
            break;
            
        case SYSTEM_STATE_USB_SUSPEND:
            break;
            
        case SYSTEM_STATE_USB_RESUME:
            break;
    }
}

void __interrupt(low_priority) SYS_InterruptLow(void)
{   
    NOP();
    NOP();
}

void __interrupt(high_priority) SYS_InterruptHigh(void)
{
    if (INTCONbits.TMR0IF)
        {
        INTCONbits.TMR0IF=0;
        T0CONbits.TMR0ON=0;
        TMR0L=0x92;
        TMR0H=0xFE;
        T0CONbits.TMR0ON=1;
        
        //ProcessIO();
        }

    #if defined(USB_INTERRUPT)
        USBDeviceTasks();
    #endif  
}

