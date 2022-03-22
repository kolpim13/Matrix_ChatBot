#include <Arduino.h>
#include "main.h"

int main(){
  RGB_Init();
  UART1_Init();
  OSC_INIT;

  RGB_Rectangle(0, 0, 30, 30, RED);
  RGB_Rectangle(1, 1, 29, 29, GREEN);
  RGB_Rectangle(2, 2, 28, 28, BLUE);
  RGB_Rectangle(3, 3, 27, 27, CIAN);
  RGB_Rectangle(4, 4, 26, 26, PURPLE);
  RGB_Rectangle(5, 5, 25, 25, YELLOW);
  RGB_Rectangle(6, 6, 24, 24, WHITE);
  //RGB_ChangeFrame();
  //RGB_Rectangle(10, 10, 25, 25, RED);

  sei();
  for(;;){
    OSC_START;
    RGB_Update();
    OSC_STOP;

    if (flag_uartMes){
      RGB_PROT_DoCommand(UART1_GetBufferIn(), UART1_pos_prev);

      UCSR0B |= ((1 << RXCIE0) | (1 << RXEN0));
      flag_uartMes = 0x00;
    }
  }
}
