#ifndef _MAIN_H
#define _MAIN_H

#include <avr/io.h>
#include <avr/interrupt.h>

#include "RGB.h"
#include "UART.h"
#include "RGB_Protocol.h"

// Oscilloscope test
#define OSC_INIT    (DDRK |= (1 << PK0))
#define OSC_START   (PORTK |= (1 << PK0))
#define OSC_STOP    (PORTK &= ~(1 << PK0))

#endif  // !_MAIN_H
