#include "Compiler.h"
#include "HardwareProfile.h"
#include "helpfunc.h"

unsigned int vsense_12v;
unsigned int vsense_3v3;
unsigned int vsense_15v;
unsigned int vsense_psu;
unsigned int an0;
unsigned int an1;
unsigned int an2;
unsigned int inputs[48];

void SetInputLatchAddress ( unsigned char v)
{
	INPUT_SELECT_BIT0 = v & 0x01;
	INPUT_SELECT_BIT1 = (v>>1) & 0x01;
	INPUT_SELECT_BIT2 = (v>>2) & 0x01;
	INPUT_SELECT_BIT3 = (v>>3) & 0x01;
}

void AllowInputs (void)
{
	INPUT_INH = 0;
}

void BlockInputs(void)
{
	INPUT_INH = 1;
}

void AllowDataOut(void)
{
	LATCH_OE=0;
}
void BlockDataOut(void)
{
	LATCH_OE=1;
}

void ClearAllPins(void)
{
	PORTD = 0;
	LATCH1 =1; LATCH1 =0;
	LATCH2 =1; LATCH2 =0;
	LATCH3 =1; LATCH3 =0;
	LATCH4 =1; LATCH4 =0;
	LATCH5 =1; LATCH5 =0;
	LATCH6 =1; LATCH6 =0;
}

void LatchData ( unsigned char data, unsigned char pin , unsigned char latch, unsigned char port_value )
{
	//unsigned char latch;
	unsigned char temp;
	
	PORTD = 0;
	LATCH1 =1; LATCH1 =0;
	LATCH2 =1; LATCH2 =0;
	LATCH3 =1; LATCH3 =0;
	LATCH4 =1; LATCH4 =0;
	LATCH5 =1; LATCH5 =0;
	LATCH6 =1; LATCH6 =0;

/*
	if (pin>=0 && pin<=7) latch=1;
	if (pin>=8 && pin<=15) latch=2;
	if (pin>=16 && pin<=23) latch=3;
	if (pin>=24 && pin<=31) latch=4;
	if (pin>=32 && pin<=39) latch=5;
	if (pin>=40 && pin<=47) latch=6;

	temp = pin - ((latch-1)*8);
	temp = 1 << temp;

	PORTD=temp;
*/
	PORTD=port_value;

	switch (latch)
		{
		case 1:
			LATCH1 =1; LATCH1 =0;
			break;
		case 2:
			LATCH3 =1; LATCH3 =0;
			break;
		case 3:
			LATCH5 =1; LATCH5 =0;
			break;
		case 4:
			LATCH2 =1; LATCH2 =0;
			break;
		case 5:
			LATCH4 =1; LATCH4 =0;
			break;
		case 6:
			LATCH6 =1; LATCH6 =0;
			break;
		}
}

/*
void Latch1 (void) { LATCH1 =1; Nop(); Nop(); LATCH1 =0; }
void Latch2 (void) { LATCH2 =1; Nop(); Nop(); LATCH2 =0; }
void Latch3 (void) { LATCH3 =1; Nop(); Nop(); LATCH3 =0; }
void Latch4 (void) { LATCH4 =1; Nop(); Nop(); LATCH4 =0; }
void Latch5 (void) { LATCH5 =1; Nop(); Nop(); LATCH5 =0; }
void Latch6 (void) { LATCH6 =1; Nop(); Nop(); LATCH6 =0; }
*/
void Latch1 (void) { LATCH1 =1; LATCH1 =0; }
void Latch2 (void) { LATCH2 =1; LATCH2 =0; }
void Latch3 (void) { LATCH3 =1; LATCH3 =0; }
void Latch4 (void) { LATCH4 =1; LATCH4 =0; }
void Latch5 (void) { LATCH5 =1; LATCH5 =0; }
void Latch6 (void) { LATCH6 =1; LATCH6 =0; }


void ReadADDataChannels( unsigned char an)
{
	unsigned char t;

	ADCON0 = ADCON0 & 0xC3;
	ADCON0 = ADCON0 | SENSE_AN0;
	vsense_12v = ReadAD();

	for (t=0;t<16;t++)
		{
		SetInputLatchAddress(t);
		inputs[t]= ReadAD();
		}

	ADCON0 = ADCON0 & 0xC3;
	ADCON0 = ADCON0 | SENSE_AN1;
	vsense_12v = ReadAD();

	for (t=0;t<16;t++)
		{
		SetInputLatchAddress(t);
		inputs[t+16]= ReadAD();
		}

	ADCON0 = ADCON0 & 0xC3;
	ADCON0 = ADCON0 | SENSE_AN2;
	vsense_12v = ReadAD();

	for (t=0;t<16;t++)
		{
		SetInputLatchAddress(t);
		inputs[t+32]= ReadAD();
		}

}
void ReadADChannels(void)
{
	unsigned char i;
	float t=0;

	//INTCONbits.GIE=0;

	ADCON0 = ADCON0 & 0xC3;
	ADCON0 = ADCON0 | SENSE_12V;
	vsense_12v = ReadAD();

	ADCON0 = ADCON0 & 0xC3;
	ADCON0 = ADCON0 | SENSE_3V3;
	vsense_3v3 = ReadAD();

	ADCON0 = ADCON0 & 0xC3;
	ADCON0 = ADCON0 | SENSE_15V;
	t = (float)ReadAD();
	t = t * 0.99;
	vsense_15v = (int)t;

	ADCON0 = ADCON0 & 0xC3;
	ADCON0 = ADCON0 | SENSE_PSU;
	vsense_psu = ReadAD();

	//INTCONbits.GIE=1;
}

unsigned int ReadAD(void)
{
	int i;
	ADCON0bits.ADON=1;
	for (i=0;i<20;i++) { Nop(); }
	Nop();
	Nop();
	ADCON0bits.GO_DONE=1;
	while (ADCON0bits.GO_DONE==1) { }
	return ADRESH*256+ADRESL;
}