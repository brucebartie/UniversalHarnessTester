#include "Compiler.h"
#include "HardwareProfile.h"
#include "helpfunc.h"
#include "init.h"

void InitRegisters(void)
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
}