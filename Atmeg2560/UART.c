#include "UART.h"

static volatile uint8_t* UART1_bufferIn;
static volatile uint16_t UART1_bufferIn_len;

static volatile uint16_t pos = 0;
volatile uint16_t UART1_pos_prev = 0;
volatile int8_t flag_uartMes = 0;
//------------------------------------------

void UART1_Init(void){
    // Baud rate 115200 if F_CPU == 16 MHz.
    //UBRR0H = 0;     //115200
    //UBRR0L = 8;     
    UBRR0H = 0;     //76800
    UBRR0L = 12;
    //UBRR0H = 0;     //9600
    //UBRR0L = 103;   
    // 8 bit data, 1 stop bit, no parity, rising edge
    UCSR0C = ((1 << UCSZ01) | (1 << UCSZ00));
    // Receiver and transmitter en
    UCSR0B = ((1 << RXEN0) | (1 << TXEN0));
    // Rec interrupt en
    UCSR0B |= (1 << RXCIE0);

    UART1_SetBufferIn(RGB_GetFrameNext(), RGB_buffer_len);
}
void UART1_Transmit(uint8_t* buf, uint8_t len){
    for (uint8_t i = 0; i < len; i++){
        while(!(UCSR0A & (1 << UDRE0)))
            ;
        UDR0 = buf[i];
    }
}

void UART1_SetBufferIn(uint8_t* buf, uint16_t len){
    UART1_bufferIn = buf;
    UART1_bufferIn_len = len;
}
uint8_t* UART1_GetBufferIn(void){
    return UART1_bufferIn;
}
//------------------------------------------

ISR(USART0_RX_vect){
    uint8_t data = UDR0;

    if (data == UART1_END_CHAR){
        flag_uartMes = 0x0F;
        UART1_pos_prev = pos;
        pos = 0;

        UCSR0B &= ~((1 << RXCIE0) | (1 << RXEN0));
    }
    else{
        UART1_bufferIn[pos] = data;
        pos++;

        if (pos >= UART1_bufferIn_len){
            pos = 0;
        }
    }

    //UDR0 = data;
}
//------------------------------------------
