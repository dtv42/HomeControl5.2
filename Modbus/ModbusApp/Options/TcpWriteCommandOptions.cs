namespace ModbusApp.Options
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.IO;
    using System.Text.Json;

    using ModbusLib;

    #endregion

    public class TcpWriteCommandOptions : TcpCommandOptions
    {
        public string Coil { get; set; } = string.Empty;
        public string Holding { get; set; } = string.Empty;
        public bool Hex { get; set; }
        public ushort Offset { get; set; } = 0;
        public string Type { get; set; } = string.Empty;

        /// <summary>
        ///  Additional check on options.
        /// </summary>
        /// <returns></returns>
        public void CheckOptions(IConsole console)
        {
            // Ignoring some of the options
            if (Hex && !string.IsNullOrEmpty(Type) && !Type.Equals("string", StringComparison.InvariantCultureIgnoreCase))
            {
                console.Out.WriteLine("HEX output option is ignored (-x can only be used with type 'string').");
            }

            if (Hex && !string.IsNullOrEmpty(Coil))
            {
                console.Out.WriteLine($"HEX output option is ignored (-x can only be used with -h -t string).");
            }

            if (!string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Coil))
            {
                console.Out.WriteLine($"Specified type '{Type}' is ignored (-t can only be used with -h).");
            }

            if (!string.IsNullOrEmpty(Coil))
            {
                if (!Coil.Contains("["))
                {
                    Coil = "[" + Coil;
                }
                if (!Coil.Contains("]"))
                {
                    Coil += "]";
                }

                List<bool>? values = JsonSerializer.Deserialize<List<bool>>(Coil);
                var number = values?.Count;

                if ((number > IModbusClient.MaxBooleanPoints))
                {
                    throw new ArgumentOutOfRangeException($"Number {number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                }
            }

            if (!string.IsNullOrEmpty(Holding))
            {
                if (!string.IsNullOrEmpty(Type) && Type.Equals("string", StringComparison.InvariantCultureIgnoreCase)) return;

                if (!Holding.Contains("["))
                {
                    Holding = "[" + Holding;
                }
                if (!Holding.Contains("]"))
                {
                    Holding += "]";
                }

                List<object>? values = JsonSerializer.Deserialize<List<object>>(Holding);
                var number = values?.Count;

                if (!string.IsNullOrEmpty(Type))
                {
                    _ = Type switch
                    {
                        "bits"   =>  (number > 1) ? throw new ArgumentOutOfRangeException($"{nameof(number)}", "Only a single bit array value is supported.") : true,
                        "string" => ((number < 1) || ((number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading string values: Number {number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "byte"   => ((number < 1) || ((number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading byte values:   Number {number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "short"  => ((number < 1) || (number > IModbusClient.MaxRegisterPoints)) ?           throw new ArgumentOutOfRangeException($"Reading short values:  Number {number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "ushort" => ((number < 1) || (number > IModbusClient.MaxRegisterPoints)) ?           throw new ArgumentOutOfRangeException($"Reading ushort values: Number {number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "int"    => ((number < 1) || (number > IModbusClient.MaxRegisterPoints / 2)) ?       throw new ArgumentOutOfRangeException($"Reading int values:    Number {number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "uint"   => ((number < 1) || (number > IModbusClient.MaxRegisterPoints / 2)) ?       throw new ArgumentOutOfRangeException($"Reading uint values:   Number {number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "float"  => ((number < 1) || (number > IModbusClient.MaxRegisterPoints / 2)) ?       throw new ArgumentOutOfRangeException($"Reading float values:  Number {number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "double" => ((number < 1) || (number > IModbusClient.MaxRegisterPoints / 4)) ?       throw new ArgumentOutOfRangeException($"Reading double values: Number {number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "long"   => ((number < 1) || (number > IModbusClient.MaxRegisterPoints / 4)) ?       throw new ArgumentOutOfRangeException($"Reading long values:   Number {number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "ulong"  => ((number < 1) || (number > IModbusClient.MaxRegisterPoints / 4)) ?       throw new ArgumentOutOfRangeException($"Reading ulong values:  Number {number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        _ => throw new ArgumentOutOfRangeException($"Unknown type '{Type}' (should not happen).")
                    };
                }
                else
                {
                    if ((number < 1) || (number > IModbusClient.MaxRegisterPoints))
                    {
                        throw new ArgumentOutOfRangeException($"Number {number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                    }
                }
            }
        }
    }
}
