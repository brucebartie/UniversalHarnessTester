#include "app_device_custom_hid.h"
#include "usb_config.h"
#include "usb_device.h"
#include "usb.h"
#include "HardwareProfile.h"
#include "helpfunc.h"
#include "main.h"

#pragma config XINST    = OFF   	// Extended instruction set
#pragma config STVREN   = ON      	// Stack overflow reset
#pragma config PLLDIV   = 4         // (12 MHz crystal used on this board)
#pragma config WDTEN    = OFF      	// Watch Dog Timer (WDT)
#pragma config CP0      = OFF      	// Code protect
#pragma config CPUDIV   = OSC1      // OSC1 = divide by 1 mode
#pragma config IESO     = OFF      	// Internal External (clock) Switchover
#pragma config FCMEN    = OFF      	// Fail Safe Clock Monitor
#pragma config FOSC     = HSPLL     // Firmware must also set OSCTUNE<PLLEN> to start PLL!
#pragma config WDTPS    = 32768
#pragma config MSSPMSK  = MSK5
#pragma config CCP2MX   = DEFAULT   

unsigned char ReceivedDataBuffer[64];
unsigned char ToSendDataBuffer[64];

unsigned char blinkStatusValid = 1;
int i_tmrIndicator;
unsigned char led_status;
unsigned char bg_flag;
unsigned char bg_counter;
unsigned char pwm_max;
unsigned char pwm_value;
unsigned char pwm_step;
unsigned char pwm_flag;
signed char pwm_step_value=10;
unsigned int ui_temp1;
unsigned int ui_temp2;

static void InitializeSystem(void);
void UserInit(void);
void YourHighPriorityISRCode();
void YourLowPriorityISRCode();

int main(void)
{
	unsigned char i;

    InitializeSystem();
	
    USBDeviceInit();
    USBDeviceAttach();

	AllowInputs();
	AllowDataOut();
	VSWITCH=0;

    while(1)
    {
        #if defined(USB_POLLING)
            USBDeviceTasks(); 
        #endif
		
        ProcessIO();
        //LED1=~LED1;            
        
        if (vsense_psu>580)
            LED4=1;
        else
            LED4=0;
        
        if((USBGetDeviceState() < CONFIGURED_STATE) || (USBIsDeviceSuspended() == true))        
            NOP();
        else
            APP_DeviceCustomHIDTasks();  
    }
}

static void InitializeSystem(void)
{
	TRISA = PORTA_TRIS;
	TRISB = PORTB_TRIS;
	TRISC = PORTC_TRIS;
	TRISD = PORTD_TRIS;
	TRISE = PORTE_TRIS;
	TRISF = PORTF_TRIS;
	TRISG = PORTG_TRIS;
	TRISH = PORTH_TRIS;
	TRISJ = PORTJ_TRIS;

    WDTCONbits.ADSHR = 0;			// Select alternate SFR location to access ANCONx registers
	ADCON0 =1;
	ADCON1 =0x8E;
    WDTCONbits.ADSHR = 1;
	ANCON0 =0;
	ANCON1 =0;
    WDTCONbits.ADSHR = 0;

	BUZZER_DRIVE=0;
	LED2=0;
	LED3=0;
	LED4=0;
    
    ADCON1 |= 0x0F;                 // Default all pins to digital

    unsigned int pll_startup_counter = 600;
    OSCTUNEbits.PLLEN = 1;  //Enable the PLL and wait 2+ms until the PLL locks before enabling USB module
    while(pll_startup_counter--);

    WDTCONbits.ADSHR = 1;			// Select alternate SFR location to access ANCONx registers
    ANCON0 = 0xFF;                  // Default all pins to digital
    ANCON1 = 0xFF;                  // Default all pins to digital
    WDTCONbits.ADSHR = 0;			// Select normal SFR locations

    #if defined(USE_USB_BUS_SENSE_IO)
    tris_usb_bus_sense = USB_BUS_SENSE_PIN; // See HardwareProfile.h
    #endif
    
    #if defined(USE_SELF_POWER_SENSE_IO)
    tris_self_power = 1;	// See HardwareProfile.h
    #endif

    PSU_ON_OFF=0;

	T0CONbits.T08BIT=0;
	T0CONbits.PSA=0;
	T0CONbits.T0CS=0;
	T0CONbits.T0PS2=0;
	T0CONbits.T0PS1=1;
	T0CONbits.T0PS0=1;
	T0CONbits.TMR0ON=1;
	INTCONbits.TMR0IF=0;
	INTCONbits.TMR0IE=1;
    
    led_status=0;
	bg_flag=0;
	pwm_value=0;
	pwm_max=180;
	pwm_step=0;
	pwm_step_value=10;
	pwm_flag=0;
    
}//end InitializeSystem

void ProcessIO(void)
{   
	switch (bg_flag)
	{
		case 0: 
			if (pwm_flag)
				{
				bg_flag=1;
				pwm_flag=0;
				}
			else
				{
				pwm_step=pwm_step+pwm_step_value;
				pwm_value=0;
				bg_flag=1;
				LED1=1;
				}
			break;
			
		case 1:
			pwm_value++;
			if (pwm_value>=pwm_step) 
				{
				bg_flag=2;
				LED1=0;
				}
			break;
			
		case 2:
			pwm_value++;
			if (pwm_value>=pwm_max) bg_flag=3;
			break;	
			
		case 3:
			bg_flag=0;				
			pwm_value=0;
			if (pwm_step_value>0)
				{
					if (pwm_step==pwm_max)
						pwm_step_value=-1;
				}
			else
				{
					if (pwm_step==0)
						{
						pwm_step_value=1;
						pwm_flag=1;
						}
				}
			break;
	}
}




