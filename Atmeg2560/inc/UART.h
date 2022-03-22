#ifndef _UART_H
#define _UART_H

#include "main.h"

#define UART1_END_CHAR       255 //('q')
//------------------------------------------

extern volatile uint16_t UART1_pos_prev;
extern volatile int8_t flag_uartMes;
//------------------------------------------

void UART1_Init(void);
void UART1_Transmit(uint8_t* buf, uint8_t len);

void UART1_SetBufferIn(uint8_t* buf, uint16_t len);
uint8_t* UART1_GetBufferIn(void);
//------------------------------------------

#endif  // !_UART_H
