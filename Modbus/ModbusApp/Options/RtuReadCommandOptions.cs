namespace ModbusApp.Options
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.IO;

    using ModbusLib;

    #endregion

    public class RtuReadCommandOptions : RtuCommandOptions
    {
        public bool Coil { get; set; }
        public bool Discrete { get; set; }
        public bool Holding { get; set; }
        public bool Input { get; set; }
        public bool Hex { get; set; }
        public ushort Number { get; set; }
        public ushort Offset { get; set; }
        public string Type { get; set; } = string.Empty;

        public void CheckOptions(IConsole console)
        {
            // Ignoring some of the options
            if (Hex && !string.IsNullOrEmpty(Type) && !Type.Equals("string", StringComparison.InvariantCultureIgnoreCase))
            {
                console.Out.WriteLine("HEX output option is ignored (-x can only be used with type 'string').");
            }

            if (Hex && (Coil || Discrete))
            {
                console.Out.WriteLine($"HEX output option is ignored (-x can only be used with -h or -i).");
            }

            if (!string.IsNullOrEmpty(Type) && (Coil || Discrete))
            {
                console.Out.WriteLine($"Specified type '{Type}' is ignored (-t can only be used with -h or -i).");
            }

            if (Coil || Discrete)
            {
                if ((Number < 1) || (Number > IModbusClient.MaxBooleanPoints))
                {
                    throw new ArgumentOutOfRangeException($"Number {Number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                }
            }

            if (Holding || Input)
            {
                if (!string.IsNullOrEmpty(Type))
                {
                    _ = Type switch
                    {
                        "bits" => (Number > 1) ? throw new ArgumentOutOfRangeException($"{nameof(Number)}", "Only a single bit array value is supported.") : true,
                        "string" => ((Number < 1) || ((Number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading string values: Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "byte" => ((Number < 1) || ((Number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading byte values:   Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "short" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading short values:  Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "ushort" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading ushort values: Number {Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "int" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading int values:    Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "uint" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading uint values:   Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "float" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading float values:  Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "double" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading double values: Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "long" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading long values:   Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        "ulong" => ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading ulong values:  Number {Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                        _ => throw new ArgumentOutOfRangeException($"Unknown type '{Type}' (should not happen).")
                    };
                }
                else
                {
                    if ((Number < 1) || (Number > IModbusClient.MaxRegisterPoints))
                    {
                        throw new ArgumentOutOfRangeException($"Number {Number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                    }
                }
            }
        }
    }
}
