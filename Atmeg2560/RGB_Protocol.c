#include "RGB_Protocol.h"

RGB_Status RGB_PROT_DoCommand(uint8_t command_buffer[], uint8_t buffer_len){
    uint8_t command = command_buffer[COM_POS];
    switch (command)
    {   
        case COM_Picture:
            return RGB_PROT_Picture(command_buffer, buffer_len);
        default:
            return RGB_WRONG_COMMAND;
    }
}

RGB_Status RGB_PROT_Picture(uint8_t command_buffer[], uint8_t buffer_len){
    RGB_ChangeFrame();
    UART1_SetBufferIn(RGB_GetFrameNext(), RGB_buffer_len);
    return RGB_OK;
}
