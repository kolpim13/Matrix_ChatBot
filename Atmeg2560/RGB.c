#include "RGB.h"

static uint8_t rgb_buffer1[RGB_buffer_len];
static uint8_t rgb_buffer2[RGB_buffer_len];
static volatile uint8_t* rgb_buffer = rgb_buffer1;
//------------------------------------------

void RGB_Init(void){
    // Инициализация пинов для матрици
	DDRB |= ((1 << CLK) | (1 << LAT));
	DDRH |= (1 << OE);
	DDRF |= (1 << A) | (1 << B) | (1 << C) | (1 << D);
	DDRA |= (1 << R1) | (1 << G1) | (1 << B1) | (1 << R2) | (1 << G2) | (1 << B2);
	PORTH &= ~(1 << OE);
}
void RGB_Update(void){
    for (uint8_t row = 0; row < RGB_heightH; row++){
        // Every new row
        PORTB |= (1 << LAT);
		PORTB &= ~(1 << LAT);

        uint16_t pos = row * RGB_width + RGB_command_len;
        for (uint8_t col = 0; col < RGB_width; col++){
            uint8_t port = (rgb_buffer[pos + col] & RGB12_mask);
            PORTA = port;

            // every new col in current row
            PORTB &= ~(1 << CLK);
			PORTB |= (1 << CLK);
        }

        // New sequence for row with saving state of half F port
        uint8_t temp = PORTF;
        temp &= RGB_row_mask_inv;
        temp |= row;
        PORTF = temp;
    }
}
//------------------------------------------

void RGB_ChangeFrame(void){
    if (rgb_buffer == rgb_buffer1){
        rgb_buffer = rgb_buffer2;
    }
    else{
        rgb_buffer = rgb_buffer1;
    }
}
uint8_t* RGB_GetFrame(void){
    return rgb_buffer;
}
uint8_t* RGB_GetFrameNext(void){
    if (rgb_buffer == rgb_buffer1){
        return rgb_buffer2;
    }
    return rgb_buffer1;
}
//------------------------------------------

RGB_Status RGB_Pixel(uint8_t x, uint8_t y, COLOR color){
    uint8_t row = y % 16;
    uint16_t index = row * RGB_width + x + RGB_command_len;
    uint8_t oldValue = rgb_buffer[index];
    if (y < 16){
        oldValue &= RGB2_mask;
        oldValue |= (uint8_t)color;
    }
    else{
        oldValue &= RGB1_mask;
        oldValue |= (uint8_t)(color << 3);
    }
    rgb_buffer[index] = oldValue;

    return RGB_OK;
}
RGB_Status RGB_Rectangle(uint8_t x0, uint8_t y0, uint8_t x1, uint8_t y1, COLOR color){
    if (!RGB_CheckRange_1(x0, y0) || !RGB_CheckRange_1(x1, y1)){
        return RGB_ARG_OUT_OF_RANGE;
    }
    
    for (uint8_t row = y0; row <= y1; row++){
        RGB_Pixel(row, x0, color); //RGB_Pixel_M(row, x0, color); 
        RGB_Pixel(row, x1, color); //RGB_Pixel_M(row, x1, color);
    }
    for (uint8_t col = x0; col <= x1; col++){
        RGB_Pixel(y0, col, color); //RGB_Pixel_M(y0, col, color);
        RGB_Pixel(y1, col, color); //RGB_Pixel_M(y1, col, color);
    }

    return RGB_OK;
}
//------------------------------------------
