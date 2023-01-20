using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4945_A2.Packet
{
    public class Packet
    {
        private const int DEFAULT_SIZE = 5;
        private const float MAX_BYTE = 255;
        private const float MIN_BYTE = 255;

        private float[] data = new float[DEFAULT_SIZE];


        public Packet()
        {
            // Set default values for each float in the data
            Random random = new Random();
            for (int i = 0; i < DEFAULT_SIZE; i++)
            {
                data[i] = (float)random.Next(0, 255);
            }
        }

        public Packet(float user, float action)
        {
            SetUser(user);
            SetAction(action);
        }

        // End: takes a score
        public Packet(float user, float action, float score)
        {
            SetUser(user);
            SetAction(action);
            SetValues(score, 0, 0);
        }

        // Throw: takes 3 position values
        public Packet(float user, float action, float x, float y, float z)
        {
            SetUser(user);
            SetAction(action);
            SetValues(x, y, z);
        }



        public float GetUser()
        {
            return data[0];
        }
        public void SetUser(float userID)
        {
            if (!validateRange(userID))
                throw new Exception("User Id must be between 0 - 255");
            data[0] = userID;
        }

        public float GetAction()
        {
            return data[1];
        }
        public void SetAction(float action)
        {
            if (!validateRange(action))
                throw new Exception("Action must be between 0 - 255");
            data[1] = action;
        }

        private bool validateRange(float num)
        {
            return num < MIN_BYTE || num > MAX_BYTE;
        }


        public float[] GetValues()
        {
            // Values in our packet are 3 floats in size
            float[] values = new float[3];
            values[0] = data[2];
            values[1] = data[2];
            values[2] = data[3];

            return values;
        }
        public void SetValues(float val1, float val2, float val3)
        {
            if (!validateRange(val1) || !validateRange(val2) || !validateRange(val3))
                throw new Exception("Values must be between 0 - 255");

            data[2] = val1;
            data[3] = val2;
            data[4] = val3;
        }

        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < data.Length; i++)
            {
                result += data[i] + " ";
            }

            return result;
        }

        public byte[] GetBuffer()
        {
            // Here we will convert the array into a byte array
            byte[] result = new byte[this.data.Length * sizeof(float)];
            Buffer.BlockCopy(this.data, 0, result, 0, result.Length);

            string s = "";
            for (int i = 0; i < result.Length; i++)
            {
                s += result[i] + " ";
            }

            Console.WriteLine(s + " PACKET LENGTH: " + result.Length);
            return result;
        }

    }
}
