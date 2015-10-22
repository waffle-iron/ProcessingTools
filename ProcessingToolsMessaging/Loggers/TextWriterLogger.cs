﻿namespace ProcessingTools.Globals.Loggers
{
    using System;
    using System.IO;

    public class TextWriterLogger : ILogger
    {
        private TextWriter textWriter;

        public TextWriterLogger()
            : this(Console.Out)
        {
        }

        public TextWriterLogger(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Log()
        {
            try
            {
                Console.WriteLine();
            }
            catch (IOException)
            {
            }
            catch
            {
                throw;
            }
        }

        public void Log(object message)
        {
            try
            {
                Console.WriteLine(message);
            }
            catch (IOException)
            {
            }
            catch
            {
                throw;
            }
        }

        public void Log(string format, params object[] args)
        {
            try
            {
                Console.WriteLine(format, args);
            }
            catch (IOException)
            {
            }
            catch
            {
                throw;
            }
        }

        public void Log(LogType type, object message)
        {
            try
            {
                this.SetLogTypeColor(type);
                Console.WriteLine("{0}: {1}", Diagnostics.GetCurrentMethod(2), type.ToString());
                Console.WriteLine(message);
                Console.ResetColor();
            }
            catch (IOException)
            {
            }
            catch
            {
                throw;
            }
        }

        public void Log(Exception e, object message)
        {
            this.Log(LogType.Exception, e, message);
        }

        public void Log(LogType type, string format, params object[] args)
        {
            try
            {
                this.SetLogTypeColor(type);
                Console.WriteLine("{0}: {1}", Diagnostics.GetCurrentMethod(2), type.ToString());
                Console.WriteLine(format, args);
                Console.ResetColor();
            }
            catch (IOException)
            {
            }
            catch
            {
                throw;
            }
        }

        public void Log(LogType type, Exception e, object message)
        {
            try
            {
                this.SetLogTypeColor(type);
                Console.WriteLine("{0}: {1}: {2}: {3}", Diagnostics.GetCurrentMethod(2), type.ToString(), e.GetType(), e.Message);
                Console.WriteLine(message);
                Console.ResetColor();
            }
            catch (IOException)
            {
            }
            catch
            {
                throw;
            }
        }

        public void Log(Exception e, string format, params object[] args)
        {
            this.Log(LogType.Exception, e, format, args);
        }

        public void Log(LogType type, Exception e, string format, params object[] args)
        {
            try
            {
                this.SetLogTypeColor(type);
                Console.WriteLine("{0}: {1}: {2}: {3}", Diagnostics.GetCurrentMethod(2), type.ToString(), e.GetType(), e.Message);
                Console.WriteLine(format, args);
                Console.ResetColor();
            }
            catch (IOException)
            {
            }
            catch
            {
                throw;
            }
        }

        private void SetLogTypeColor(LogType type)
        {
            switch (type)
            {
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case LogType.Exception:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                default:
                    break;
            }
        }
    }
}