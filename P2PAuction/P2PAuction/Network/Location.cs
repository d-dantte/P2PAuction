using P2PAuction.Utils;
using System.Net;
using System.Text.RegularExpressions;

namespace P2PAuction.Network
{
    public readonly struct Location
    {
        private static readonly Regex Pattern = new("^0x[a-fA-F0-9]{2}( [a-fA-F0-9]{2}){0,15}:\\d+$");

        private readonly byte _b0;
        private readonly byte _b1;
        private readonly byte _b2;
        private readonly byte _b3;

        private readonly byte _b4;
        private readonly byte _b5;
        private readonly byte _b6;
        private readonly byte _b7;

        private readonly byte _b8;
        private readonly byte _b9;
        private readonly byte _b10;
        private readonly byte _b11;

        private readonly byte _b12;
        private readonly byte _b13;
        private readonly byte _b14;
        private readonly byte _b15;

        private readonly int _port;

        public Location(byte[] addressBytes, int port)
        {
            ArgumentNullException.ThrowIfNull(addressBytes);

            if (port < 0)
                throw new ArgumentException($"Invalid port: {port}");

            _port = port;

            _b0 = addressBytes.Length > 0 ? addressBytes[0] : default;
            _b1 = addressBytes.Length > 1 ? addressBytes[1] : default;
            _b2 = addressBytes.Length > 2 ? addressBytes[2] : default;
            _b3 = addressBytes.Length > 3 ? addressBytes[3] : default;
            _b4 = addressBytes.Length > 4 ? addressBytes[4] : default;
            _b5 = addressBytes.Length > 5 ? addressBytes[5] : default;
            _b6 = addressBytes.Length > 6 ? addressBytes[6] : default;
            _b7 = addressBytes.Length > 7 ? addressBytes[7] : default;
            _b8 = addressBytes.Length > 8 ? addressBytes[8] : default;
            _b9 = addressBytes.Length > 9 ? addressBytes[9] : default;
            _b10 = addressBytes.Length > 10 ? addressBytes[10] : default;
            _b11 = addressBytes.Length > 11 ? addressBytes[11] : default;
            _b12 = addressBytes.Length > 12 ? addressBytes[12] : default;
            _b13 = addressBytes.Length > 13 ? addressBytes[13] : default;
            _b14 = addressBytes.Length > 14 ? addressBytes[14] : default;
            _b15 = addressBytes.Length > 15 ? addressBytes[15] : default;
        }

        public override string ToString()
        {
            var stack = new Stack<string>();

            stack.Push(_b0.ToString("X:2"));

            if (_b1 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b2 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b3 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b4 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b5 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b6 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b7 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b8 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b9 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b10 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b11 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b12 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b13 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b14 > 0)
                stack.Push(_b1.ToString("X:2"));

            if (_b15 > 0)
                stack.Push(_b1.ToString("X:2"));

            return $"0x{string.Join(' ', stack)}:{_port}";
        }

        public byte[] ToByteArray() => IsIPV4
            ? new[] { _b0, _b1, _b2, _b3 }
            : new[]
            {
                _b0, _b1, _b2, _b3,
                _b4, _b5, _b6, _b7,
                _b8, _b9, _b10, _b11,
                _b12, _b13, _b14, _b15
            };

        public IPAddress GetIPAddress() => new(ToByteArray());

        public int Port => _port;

        public bool IsIPV4
            => _b4 == 0 && _b5 == 0
            && _b6 == 0 && _b7 == 0
            && _b8 == 0 && _b9 == 0
            && _b10 == 0 && _b11 == 0
            && _b12 == 0 && _b13 == 0
            && _b14 == 0 && _b15 == 0;


        public static implicit operator Location(
            string value)
            => Parse(value);

        public static implicit operator string(
            Location address)
            => address.ToString()!;

        /// <summary>
        /// Parses a string from the format <c>0x00 00 00 ...00</c>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Location Parse(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!Pattern.IsMatch(value))
                throw new FormatException($"Invalid address format: {value}");

            var parts = value.Split(':');

            return parts[0][2..]
                .Split(' ')
                .Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber))
                .ToArray()
                .ApplyTo(bytes => new Location(bytes, int.Parse(parts[1])));
        }
    }
}
