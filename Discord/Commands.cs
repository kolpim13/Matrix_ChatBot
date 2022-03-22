using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace ChatBot
{
    public enum RGB_Color
    {
        Black = 0,
        Red = 1,
        Green = 2,
        Yellow = 3,
        Blue = 4,
        Purple = 5,
        Cian = 6,
        White = 7,
    }

    public enum RGB_Command
    {
        Pixel = 0,
        Rectangle = 1,
        RectangleF = 2,
        Circle = 3,
        CircleF = 4,
        Fill = 5,
        Picture = 100,
    }

    // [Remainder]
     
    // Keep in mind your module **must** be public and inherit ModuleBase.
    // If it isn't, it will not be discovered by AddModulesAsync!
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [Summary("Check if bot works")]
        public Task SayAsync() => 
            ReplyAsync("Test OK");

        [Command("help")]
        [Summary("List of all available commands")]
        public async Task HelpAsync()
        {
            string response = "Info Commands: help; test. \n" +
                "RGB Commands: \n " +
                "line(x0, y0, x1, y1); \n " +
                "square(x0, y0, x1, y1); \n " +
                "circle(x_center, y_center, radius); \n " +
                "text(\"some string\") \n";

            await Context.Channel.SendMessageAsync(response);
        }
    }

    [Group("UART")]
    public class UARTModule : ModuleBase<SocketCommandContext>
    {
        [Command("open")]
        [Summary("Open COM port")]
        public async Task Port_openAsync(string port_name)
        {
            if (UART.Open(port_name) == true)
            {
                await Context.Channel.SendMessageAsync("Operation success");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Operation fail");
            }        
        }

        [Command("close")]
        [Summary("Close COM port")]
        public async Task Port_closeAsync()
        {
            if (UART.Close() == true)
            {
                await Context.Channel.SendMessageAsync("Operation success");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Operation fail");
            }
        }

        [Command("port name")]
        [Summary("")]
        public async Task Port_nameAsync(string port_name)
        {
            UART.PortName = port_name;
            await Context.Channel.SendMessageAsync("Port name changed");
        }

        [Command("get_ports")]
        [Summary("")]
        public async Task Get_portsAsync()
        {
            string[] ports = UART.GetPortNames;
            await Context.Channel.SendMessageAsync("Awailable ports:\n");
            foreach (string str in ports)
            {
                await Context.Channel.SendMessageAsync(str);
            }
        }
    }

    [Group("RGB")]
    public class RGBModule : ModuleBase<SocketCommandContext>
    {
        [Command("line")]
        [Summary("Draw a broken line")]
        public async Task LineAsync([Summary("2 pairs x-y to connect")]
            int x0, int y0, int x1, int y1)
        {
            await Context.Channel.SendMessageAsync("RGB line bla bla");

            // Посмотреть как тут можно сделать ожидание по таймуту, что бы рапортовать об ошибке!
        }

        [Command("fill")]
        [Summary("Fill whole matrix with some color")]
        public async Task FillAsync(RGB_Color color)
        {
            UART.Get_buf_out[0] = (byte)RGB_Command.Fill;
            UART.Get_buf_out[1] = (byte)color;
            UART.Get_buf_out[2] = 255;

            // await Task.Run(() => UART.Write(3));
            UART.Write(3);
        }

        [Command("pixel")]
        [Summary("Set a single pixel")]
        public async Task PixelAsync(byte x, byte y, RGB_Color color)
        {
            UART.Get_buf_out[0] = (byte)RGB_Command.Pixel;
            UART.Get_buf_out[1] = x;
            UART.Get_buf_out[2] = y;
            UART.Get_buf_out[3] = (byte)color;
            UART.Get_buf_out[4] = 255;

            // await Task.Run(() => UART.Write(5));
            UART.Write(5);
        }

        [Command("rectangle")]
        [Summary("Draw a rectangle on matrix")]
        public async Task RectangleAsync([Summary("x-y left top and right bottom")] 
            byte x0, byte y0, byte x1, byte y1, RGB_Color color)
        {
            UART.Get_buf_out[0] = (byte)RGB_Command.Rectangle;
            UART.Get_buf_out[1] = x0;
            UART.Get_buf_out[2] = y0;
            UART.Get_buf_out[3] = x1;
            UART.Get_buf_out[4] = y1;
            UART.Get_buf_out[5] = (byte)color;
            UART.Get_buf_out[6] = 255;

            // await Task.Run(() => UART.Write(7));
            UART.Write(7);
        }

        [Command("rectangleF")]
        [Summary("Draw a rectangle on matrix and fill it")]
        public async Task RectangleFAsync([Summary("x-y left top and right bottom")]
            byte x0, byte y0, byte x1, byte y1, RGB_Color color)
        {
            UART.Get_buf_out[0] = (byte)RGB_Command.RectangleF;
            UART.Get_buf_out[1] = x0;
            UART.Get_buf_out[2] = y0;
            UART.Get_buf_out[3] = x1;
            UART.Get_buf_out[4] = y1;
            UART.Get_buf_out[5] = (byte)color;
            UART.Get_buf_out[6] = 255;

            //await Task.Run(() => UART.Write(7));
            UART.Write(7);
        }

        [Command("circle")]
        [Summary("Draw a circle on matrix")]
        public async Task CircleAsync([Summary("x-y center and radius")]
            byte x, byte y, byte r, RGB_Color color)
        {
            UART.Get_buf_out[0] = (byte)RGB_Command.Circle;
            UART.Get_buf_out[1] = x;
            UART.Get_buf_out[2] = y;
            UART.Get_buf_out[3] = r;
            UART.Get_buf_out[4] = (byte)color;
            UART.Get_buf_out[5] = 255;

            // await Task.Run(() => UART.Write(6));
            UART.Write(6);
        }

        [Command("circleF")]
        [Summary("Draw a circle on matrix and fill it")]
        public async Task CircleFAsync([Summary("x-y center and radius")]
            byte x, byte y, byte r, RGB_Color color)
        {
            UART.Get_buf_out[0] = (byte)RGB_Command.CircleF;
            UART.Get_buf_out[1] = x;
            UART.Get_buf_out[2] = y;
            UART.Get_buf_out[3] = r;
            UART.Get_buf_out[4] = (byte)color;
            UART.Get_buf_out[5] = 255;

            // await Task.Run(() => UART.Write(6));
            UART.Write(6);
        }

        [Command("picture")]
        [Summary("Draw a picture from given on pc")]
        public async Task PictureAsync([Summary("string consist of picture name and additional parameters")]
            string command)
        {
            // Check if there is a command 
            if (command == "" || command == null)
            {
                await Context.Channel.SendMessageAsync("Error. Try to check args for command\n");
                return;
            }

            // Parse the name of the picture and additional options
            string[] options = command.Split(" ");
            string name = options[0];

            // Convert choosen picture to our code
            Image im = new Image();
            if (im.ConvertToByteArray_BlackAndWhite(name, out byte[] pic) == false)
            {
                await Context.Channel.SendMessageAsync("File do not exist or corrupted");
                return;
            }

            // Send it to the controller string by string
            for (int row = 0; row < im.Height; row++)
            {
                UART.Get_buf_out[0] = (byte)RGB_Command.Picture;
                UART.Get_buf_out[1] = (byte)row;
                Array.Copy(pic, row * im.Width, UART.Get_buf_out, 2, im.Width);
                UART.Get_buf_out[2 + im.Width] = 255;
                UART.Write(im.Width + 3);

                if ((row - 1) % 8 == 7)
                {
                    await Context.Channel.SendMessageAsync($"String {row} loaded");
                }

                await Task.Delay(250);
            }

            await Context.Channel.SendMessageAsync("Done");
        }
    }

    [Group("RGB2")]
    public class RGB2Module : ModuleBase<SocketCommandContext> {
        [Command("pic")]
        [Summary("Draw a picture second try")]
        public async Task PictureAsync([Summary("string consist of picture name and additional parameters")]
            string command)
        {
            // Check if there is a command 
            if (command == "" || command == null)
            {
                await Context.Channel.SendMessageAsync("Error. Try to check args for command\n");
                return;
            }

            // Parse the name of the picture and additional options
            string[] options = command.Split(" ");
            string name = options[0];

            // Convert choosen picture to our code
            Picture p = new Picture();
            if (p.ConvertToByteArray_Colors8(name, out byte[] pic) == false)
            {
                await Context.Channel.SendMessageAsync("File do not exist or corrupted");
                return;
            }

            // Send it to the controller fully.
            UART.Get_buf_out[0] = (byte)RGB_Command.Picture;
            UART.Write2(1);
            UART.Write2(pic, 0, pic.Length);
            UART.Get_buf_out[0] = 255;
            UART.Write2(1);

            // Success
            await Context.Channel.SendMessageAsync("Done");
        }
    }
}
