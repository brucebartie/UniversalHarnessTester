#include "HardwareProfile.h"
#include "usb.h"
#include "usb_device_hid.h"
#include "helpfunc.h"

unsigned char ReceivedDataBuffer[64];
unsigned char ToSendDataBuffer[64];
unsigned char string_to_send[64];
unsigned char  * p;
unsigned char i,j,k;
unsigned char * mem;
float   f;

volatile USB_HANDLE USBOutHandle;    
volatile USB_HANDLE USBInHandle;

void APP_DeviceCustomHIDInitialize()
{
    USBInHandle = 0;
    USBEnableEndpoint(CUSTOM_DEVICE_HID_EP, USB_IN_ENABLED|USB_OUT_ENABLED|USB_HANDSHAKE_ENABLED|USB_DISALLOW_SETUP);
    USBOutHandle = (volatile USB_HANDLE)HIDRxPacket(CUSTOM_DEVICE_HID_EP,(uint8_t*)ReceivedDataBuffer,64);
}

void APP_DeviceCustomHIDTasks()
{   
    unsigned char t;
    //Check if we have received an OUT data packet from the host
    
    if((USBDeviceState < CONFIGURED_STATE)||(USBSuspendControl==1)) return;
    
    if(HIDRxHandleBusy(USBOutHandle) == false)
    {   
        //We just received a packet of data from the USB host.
        //Check the first uint8_t of the packet to see what command the host
        //application software wants us to fulfill.
        switch(ReceivedDataBuffer[0])				//Look at the data the host sent, to see what kind of application specific command it sent.
        {
            case USB_CMD_SELECT_12VSOURCE:
                VSWITCH=0;
                break;

            case USB_CMD_SELECT_15VSOURCE:
                VSWITCH=1;
                break;

            case USB_CMD_CLEAR_OUTPUTS:
                ClearAllPins();
                break;

            case USB_CMD_SET_OUTPUT:
                ToSendDataBuffer[0] = USB_CMD_SET_OUTPUT;
                LatchData(0,ReceivedDataBuffer[1],ReceivedDataBuffer[2],ReceivedDataBuffer[3]);
                break;

            case USB_CMD_READ_INPUTS0_FOR_OUTPUT: 
                ToSendDataBuffer[0] = USB_CMD_READ_INPUTS0_FOR_OUTPUT;
                //				if (ReceivedDataBuffer[1]<100)
                //					LatchData(0,ReceivedDataBuffer[1]);
                ReadADDataChannels(0);
                for (t=0;t<16;t++)
                    {
                    ToSendDataBuffer[(t*2)+1] = inputs[t]>>8;
                    ToSendDataBuffer[(t*2)+2] = inputs[t]&0xFF;
                    }

                    if(!HIDTxHandleBusy(USBInHandle))
                        {
                        USBInHandle = HIDTxPacket(CUSTOM_DEVICE_HID_EP, (uint8_t*)&ToSendDataBuffer[0],64);
                        }
                    break;

            case USB_CMD_READ_INPUTS1_FOR_OUTPUT: 
                ToSendDataBuffer[0] = USB_CMD_READ_INPUTS1_FOR_OUTPUT;
//				if (ReceivedDataBuffer[1]<100)
//					LatchData(0,ReceivedDataBuffer[1]);
				ReadADDataChannels(1);
				for (t=0;t<16;t++)
					{
					ToSendDataBuffer[(t*2)+1] = inputs[t+16]>>8;
					ToSendDataBuffer[(t*2)+2] = inputs[t+16]&0xFF;
					}
                    if(!HIDTxHandleBusy(USBInHandle))
                    {
                    USBInHandle = HIDTxPacket(CUSTOM_DEVICE_HID_EP, (uint8_t*)&ToSendDataBuffer[0],64);
                    }
                    break;

            case USB_CMD_READ_INPUTS2_FOR_OUTPUT: 
	            ToSendDataBuffer[0] = USB_CMD_READ_INPUTS2_FOR_OUTPUT;
//				if (ReceivedDataBuffer[1]<100)
//					LatchData(0,ReceivedDataBuffer[1]);
				ReadADDataChannels(2);
				for (t=0;t<16;t++)
					{
					ToSendDataBuffer[(t*2)+1] = inputs[t+32]>>8;
					ToSendDataBuffer[(t*2)+2] = inputs[t+32]&0xFF;
					}
                if(!HIDTxHandleBusy(USBInHandle))
                {
                USBInHandle = HIDTxPacket(CUSTOM_DEVICE_HID_EP, (uint8_t*)&ToSendDataBuffer[0],64);
                }
                break;

            case USB_CMD_READ_POWER_INPUTS: 
				ReadADChannels();
	            ToSendDataBuffer[0] = USB_CMD_READ_POWER_INPUTS;
				ToSendDataBuffer[1] = vsense_12v>>8;
				ToSendDataBuffer[2] = vsense_12v&0xff;
				ToSendDataBuffer[3] = vsense_3v3>>8;
				ToSendDataBuffer[4] = vsense_3v3&0xff;
				ToSendDataBuffer[5] = vsense_15v>>8;
				ToSendDataBuffer[6] = vsense_15v&0xff;
				ToSendDataBuffer[7] = vsense_psu>>8;	
				ToSendDataBuffer[8] = vsense_psu&0xff;	

                if(!HIDTxHandleBusy(USBInHandle))
                {
                USBInHandle = HIDTxPacket(CUSTOM_DEVICE_HID_EP, (uint8_t*)&ToSendDataBuffer[0],64);
                }
                break;

            case USB_CMD_PSU_OFF: 
				ReadADChannels();
	            PSU_ON_OFF=0;
				LED3=0;
				LED4=0;
                break;

            case USB_CMD_PSU_ON: 
				ReadADChannels();
	            PSU_ON_OFF=1;
				LED3=1;
                break;

            case USB_CMD_HARNESS_TEST_PASS: 
				LED2=0;
                break;

            case USB_CMD_HARNESS_TEST_FAIL: 
				LED2=1;
                break;


        }                        
        //Re-arm the OUT endpoint, so we can receive the next OUT data packet 
        //that the host may try to send us.
        USBOutHandle = HIDRxPacket(CUSTOM_DEVICE_HID_EP, (uint8_t*)ReceivedDataBuffer, 64);
    }
}