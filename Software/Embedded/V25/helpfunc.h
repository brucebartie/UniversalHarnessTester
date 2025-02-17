extern unsigned int vsense_12v;
extern unsigned int vsense_3v3;
extern unsigned int vsense_15v;
extern unsigned int vsense_psu;
extern unsigned int an0;
extern unsigned int an1;
extern unsigned int an2;
extern unsigned int inputs[48];

void ReadADChannels(void);
unsigned int ReadAD(void);
void SetInputLatchAddress ( unsigned char v);
void AllowInputs (void);
void BlockInputs(void);
void AllowDataOut(void);
void BlockDataOut(void);
void LatchData ( unsigned char data, unsigned char pin , unsigned char latch, unsigned char port_value );
void ClearAllPins(void);
void ReadADDataChannels( unsigned char an);
void Latch1 (void);
void Latch2 (void);
void Latch3 (void);
void Latch4 (void);
void Latch5 (void);
void Latch6 (void);

