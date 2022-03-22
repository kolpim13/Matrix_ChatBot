#ifndef _RGB_PROTOCOL_H
#define _RGB_PROTOCOL_H

#include "RGB.h"
#include "main.h"

#define COM_POS         (0)

#define COM_Picture     (100)
//------------------------------------------

enum _RGB_Status RGB_PROT_DoCommand(uint8_t command_buffer[], uint8_t buffer_len);

enum _RGB_Status RGB_PROT_Picture(uint8_t command_buffer[], uint8_t buffer_len);
//------------------------------------------

#endif  // !_RGB_PROTOCOL_H
