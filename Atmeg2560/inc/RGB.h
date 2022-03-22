#ifndef _RGB_H
#define _RGB_H

#include "main.h"

// Choose color for top half of matrix
#define R1 PA0	
#define G1 PA1
#define B1 PA2

// Choose color for bot half of matrix
#define R2 PA3
#define G2 PA4
#define B2 PA5

// Color mask
#define RGB1_mask  0x07
#define RGB2_mask  0x38
#define RGB12_mask  0x3F

// Row choose
#define A PF0
#define B PF1
#define C PF2
#define D PF3
#define RGB_row_mask        0x0F
#define RGB_row_mask_inv    0xF0

// Transmittion
#define CLK PB5	// тактовый сигнал
#define LAT PB4 // управляющий сигнал
#define OE PH6	// пин контроля отображения свечения всего дисплея

// Matrix characteristics
#define RGB_width   64
#define RGB_height  32
#define RGB_heightH 16
#define RGB_command_len 1   // Кол-во байт, которые несут в себе непосредственно комманду
#define RGB_buffer_len  (RGB_width * RGB_height / 2 + RGB_command_len)

// Matrix colors
typedef enum _COLOR{
    BLACK   = 0,    // full off
    RED     = 1,    // m
    GREEN   = 2,    // m
    YELLOW  = 3,    // R + G
    BLUE    = 4,    // m
    PURPLE  = 5,    // R + B
    CIAN    = 6,    // G + B
    WHITE   = 7,    // full on
}COLOR;

// Matrix operations status possible
typedef enum _RGB_Status{
    RGB_OK,
    RGB_ARG_OUT_OF_RANGE,
    RGB_WRONG_ARGS_AMOUNT,
    RGB_WRONG_COMMAND,
}RGB_Status;
//------------------------------------------

/* Hardware part */
void RGB_Init(void);
void RGB_Update(void);
//------------------------------------------

/* Display part */
void RGB_ChangeFrame(void);
uint8_t* RGB_GetFrame(void);
uint8_t* RGB_GetFrameNext(void);
//------------------------------------------

//#define RGB_Pixel_M(row, col, color)    (rgb_buffer[row*RGB_width+col+RGB_command_len] = (uint8_t)(color))
//#define RGB_Pixel_M(row, col, color)    (rgb_buffer[row*RGB_width+col+RGB_command_len] = (uint8_t)(color << ((row/16) << 2)))
#define RGB_CheckRange_1(x, y)          ((x) < 64 && (y) < 32)
#define RGB_CheckRange_2(x, y)          (((x) < 64 && (y) < 32) ? RGB_OK : RGB_ARG_OUT_OF_RANGE)

/* Work logic part */
RGB_Status RGB_Pixel(uint8_t x, uint8_t y, COLOR color);
RGB_Status RGB_Rectangle(uint8_t x0, uint8_t y0, uint8_t x1, uint8_t y1, COLOR color);
//------------------------------------------

#endif  //_RGB_H