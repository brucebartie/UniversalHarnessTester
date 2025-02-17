/********************************************************************
 FileName:     	HardwareProfile - PIC18F85J50 PIM.h
 Dependencies:	See INCLUDES section
 Processor:	PIC18 USB Microcontrollers
 Hardware:	PIC18F85J50 PIM
 Compiler:  	Microchip C18
 Company:		Microchip Technology, Inc.

 Software License Agreement:

 The software supplied herewith by Microchip Technology Incorporated
 (the “Company”) for its PIC® Microcontroller is intended and
 supplied to you, the Company’s customer, for use solely and
 exclusively on Microchip PIC Microcontroller products. The
 software is owned by the Company and/or its supplier, and is
 protected under applicable copyright laws. All rights are reserved.
 Any use in violation of the foregoing restrictions may subject the
 user to criminal sanctions under applicable laws, as well as to
 civil liability for the breach of the terms and conditions of this
 license.

 THIS SOFTWARE IS PROVIDED IN AN “AS IS” CONDITION. NO WARRANTIES,
 WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED
 TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,
 IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR
 CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.

********************************************************************
 File Description:

 Change History:
  Rev   Date         Description
  1.0   11/19/2004   Initial release
  2.1   02/26/2007   Updated for simplicity and to use common
                     coding style
  2.3   09/15/2008   Broke out each hardware platform into its own
                     "HardwareProfile - xxx.h" file
********************************************************************/

#ifndef HARDWARE_PROFILE_PIC18F85J50TST_H
#define HARDWARE_PROFILE_PIC18F85J50TST_H

    /*******************************************************************/
    /******** USB stack hardware selection options *********************/
    /*******************************************************************/
    //This section is the set of definitions required by the MCHPFSUSB
    //  framework.  These definitions tell the firmware what mode it is
    //  running in, and where it can find the results to some information
    //  that the stack needs.
    //These definitions are required by every application developed with
    //  this revision of the MCHPFSUSB framework.  Please review each
    //  option carefully and determine which options are desired/required
    //  for your application.

    /** USB ************************************************************/
    //The PIC18F85J50 FS USB Plug-In Module supports the USE_USB_BUS_SENSE_IO
    //feature.  The USE_SELF_POWER_SENSE_IO feature is not implemented on the
    //circuit board, so the USE_SELF_POWER_SENSE_IO define should always be
    //commented for this hardware platform.

    //#define USE_SELF_POWER_SENSE_IO
    #define tris_self_power     TRISAbits.TRISA2    // Input
    #if defined(USE_SELF_POWER_SENSE_IO)
    #define self_power          PORTAbits.RA2
    #else
    #define self_power          1
    #endif

    //#define USE_USB_BUS_SENSE_IO		//JP1 must be in R-U position to use this feature on this board	
    #define tris_usb_bus_sense  TRISBbits.TRISB5    // Input
    #if defined(USE_USB_BUS_SENSE_IO)
    #define USB_BUS_SENSE       PORTBbits.RB5
    #else
    #define USB_BUS_SENSE       PORTCbits.RC0
    #endif


   
    //Uncomment this to make the output HEX of this project 
    //   to be able to be bootloaded using the HID bootloader
	#define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER		

    /*******************************************************************/
    /*******************************************************************/
    /*******************************************************************/
    /******** Application specific definitions *************************/
    /*******************************************************************/
    /*******************************************************************/
    /*******************************************************************/

    /** Board definition ***********************************************/
    //These defintions will tell the main() function which board is
    //  currently selected.  This will allow the application to add
    //  the correct configuration bits as wells use the correct
    //  initialization functions for the board.  These defitions are only
    //  required in the stack provided demos.  They are not required in
    //  final application design.
    #define DEMO_BOARD PIC18F85J50TST
    #define PIC18F85J50TST
    #define CLOCK_FREQ 20000000
  
	// -- PORTA ----------------------------
	#define V_SENSE_12V	PORTAbits.RA0
	#define V_SENSE_3V3 PORTAbits.RA1
	#define V_SENSE_PSU	PORTAbits.RA5
	// -------------------------------------
	//0 - 12V Sense
	//1 - 3V3 Sense
	//2 - NC
	//3 - NC
	//4 - NC
	//5 - PSU Sense
	//6 - x
	//7 - x
	// 00100011 = 0x23
	#define PORTA_TRIS	0x23

	// -- PORTB ----------------------------
	// -------------------------------------
	//0 - x
	//1 - x
	//2 - x
	//3 - x
	//4 - x
	//5 - x
	//6 - RB6
	//7 - RB7
	// 11000000 = 0xC0
	#define PORTB_TRIS	0xC0

	// -- PORTC ----------------------------
	#define PSU_PWM	PORTCbits.RC2
	#define LED3 	PORTCbits.RC3
	#define LED2 	PORTCbits.RC4
	#define LED1	PORTCbits.RC5
	#define	VSWITCH	PORTCbits.RC7
	// -------------------------------------
	//0 - VBUS Sense
	//1 - x
	//2 - PSU_PWM
	//3 - LED3
	//4 - LED2
	//5 - LED1
	//6 - x
	//7 - V_SWITCH
	// 00000001 = 0x01
	#define PORTC_TRIS	0x01

	// -- PORTD ----------------------------
	// -------------------------------------
	//0 - x
	//1 -	x
	//2 - x
	//3 - x
	//4 - LIGHTS_DRIVE
	//5 -	BRAKES_DRIVE
	//6 - RF_DRIVE
	//7 - LF_DRIVE
	// 0000 0000 = 0x00
	#define PORTD_TRIS	0x00

	// -- PORTE ----------------------------
	#define LATCH3		PORTEbits.RE2
	#define LATCH4		PORTEbits.RE3
	#define LATCH5		PORTEbits.RE4
	#define LATCH6		PORTEbits.RE5
	#define LATCH_OE	PORTEbits.RE6
	// -------------------------------------
	//0 - x
	//1 - x
	//2 - LATCH 3
	//3 - LATCH 4
	//4 - LATCH 5
	//5 - LATCH 6
	//6 - LATCH OE
	//7 - x
	// 00000000 = 0x00
	#define PORTE_TRIS	0x00

	// -- PORTF ----------------------------
	#define AN0TO15			PORTFBits.RF6
	#define AN16TO31		PORTFBits.RF5
	#define AN32TO47		PORTFBits.RF2
	// -------------------------------------
	//0 - x
	//1 - x
	//2 - AN2
	//3 - D+
	//4 - D-
	//5 - AN1
	//6 - AN0
	//7 - x
	// 11111111 = 0xFF
	#define PORTF_TRIS	0xFF

	// -- PORTG ----------------------------
	#define INPUT_SELECT_BIT0	PORTGbits.RG0
	#define INPUT_SELECT_BIT1	PORTGbits.RG1
	#define INPUT_SELECT_BIT2	PORTGbits.RG2
	#define INPUT_SELECT_BIT3	PORTGbits.RG3
	// -------------------------------------
	//0 - AN Seelect Bit 0
	//1 - AN Seelect Bit 1
	//2 - AN Seelect Bit 2
	//3 - AN Seelect Bit 3
	//4 - x
	//5 - x
	//6 - x
	//7 - x
	// 11110000 = 0xF0
	#define PORTG_TRIS	0xF0

	// -- PORTH ----------------------------
	#define LATCH2				PORTHbits.RH0
	#define LATCH1				PORTHbits.RH1
	#define INPUT_INH			PORTHbits.RH2
	#define V_SENSE_PSU_OUTPUT	PORTHbits.RH6
	#define V_SENSE_15V			PORTHbits.RH7
	// -------------------------------------
	//0 - LATCH2
	//1 - LATCH1
	//2 - INPUT_SELECT_INHIBIT
	//3 - x
	//4 - x
	//5 - x
	//6 - VSENSE_PSU_OUTPUT
	//7 - VSENSE_15V
	// 11000000 = 0xC0
	#define PORTH_TRIS	0xC0

	// -- PORTJ ----------------------------
	#define LED4			PORTJbits.RJ7
	#define PSU_ON_OFF		PORTJbits.RJ5
	#define BUZZER_DRIVE	PORTJbits.RJ2
	// -------------------------------------
	//0 - x
	//1 - x
	//2 - BUZZER_DRIVE
	//3 - x
	//4 - x
	//5 - PSU_ON_OFF
	//6 - x
	//7 - LED4
	// 00000000
	#define PORTJ_TRIS	0x00

  
    /** I/O pin definitions ********************************************/
    #define INPUT_PIN 1
    #define OUTPUT_PIN 0


	// USB Related
	// ------------------------------------------------------------------------------------------
	// USB Command IDs
	#define USB_CMD_READ_INPUTS0_FOR_OUTPUT			0x80
	#define USB_CMD_READ_INPUTS1_FOR_OUTPUT			0x81
	#define USB_CMD_READ_INPUTS2_FOR_OUTPUT			0x82
	#define USB_CMD_SET_OUTPUT						0x83
	#define USB_CMD_CLEAR_OUTPUTS					0x84
	
	#define USB_CMD_READ_POWER_INPUTS				0x85
	#define USB_CMD_PSU_OFF							0x86
	#define USB_CMD_PSU_ON							0x87

	#define USB_CMD_SELECT_12VSOURCE				0x88
	#define USB_CMD_SELECT_15VSOURCE				0x89

	#define USB_CMD_HARNESS_TEST_PASS				0x95
	#define USB_CMD_HARNESS_TEST_FAIL				0x96


	// LED values
	#define LED_ON					0x01
	#define LED_OFF					0x00
	// RELAY values
	#define RELAY_ON				0x01
	#define RELAY_OFF				0x00

	// Error values
	#define RESULT_OK 				0x00	// no error

	// ------------------------------------------------------------------------------------------
	// A to D
	// ------------------------------------------------------------------------------------------
	#define SENSE_12V		0x00
	#define	SENSE_3V3		0x04
	#define	SENSE_15V		0x3C
	#define	SENSE_PSU		0x3C
	#define SENSE_PSU_OUT	0x10

	#define	SENSE_AN0		0x2C
	#define	SENSE_AN1		0x28
	#define	SENSE_AN2		0x1C

#endif  //HARDWARE_PROFILE_PIC18F85J50_PIM_H
