using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    class MachineEmbroidery
    {
        /* Definition used here:
         * right diagonal: diagonal with upper part on the right side
         * left diagonal: diagonal with upper part on the left side
         */

        public void CreatePathAndDstFile(Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, int sizeOfEachPixel, int horizontalSize, int verticalSize)
        {
            LinkedList<Tuple<StitchType, Tuple<int, int>>> path = CreatePath(positionsOfEachColor, horizontalSize, verticalSize);
            ConvertEmbroideryPathToDstFile(path, sizeOfEachPixel);
        }


        private void ConvertEmbroideryPathToDstFile(LinkedList<Tuple<StitchType, Tuple<int, int>>> listOfStitches, int sizeOfEachPixel)
        {
            string filePath = "D:/Downloads/newFile.dst";
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using(BinaryWriter bw = new BinaryWriter(fs, new ASCIIEncoding()))
                {
                    List<byte> bodyEncoding = GetEncodingDstFileBody(listOfStitches, sizeOfEachPixel, out int minX, out int maxX, out int minY, out int maxY);
                    List<byte> headerEncoding = GetEncodingDstFileHeader(bodyEncoding, minX, maxX, minY, maxY);

                    List<byte> allBytesOnThisFile = new List<byte>();
                    allBytesOnThisFile.AddRange(headerEncoding);
                    allBytesOnThisFile.AddRange(bodyEncoding);

                    bw.Write(allBytesOnThisFile.ToArray());
                    bw.Close();
                }
            }
        }

        private List<byte> GetEncodingDstFileBody(LinkedList<Tuple<StitchType, Tuple<int, int>>> listOfStitches, int sizeOfEachPixel, out int minX, out int maxX, out int minY, out int maxY)
        {
            //TODO: Should sizeOfEachPixel be an integer?

            List<byte> bytesList = new List<byte>();

            //X and Y position in number of pixels, not real distances
            int currentPositionX = 0;
            int currentPositionY = 0;

            minX = int.MaxValue;
            maxX = int.MinValue;
            minY = int.MaxValue;
            maxY = int.MinValue;

            while (listOfStitches.Count > 0)
            {
                Tuple<StitchType, Tuple<int, int>> currentStitch = listOfStitches.First();
                Tuple<StitchType, Tuple<int, int>> currentStitchCorrectedAxis = new Tuple<StitchType, Tuple<int, int>>(currentStitch.Item1, new Tuple<int, int>(currentStitch.Item2.Item1, -currentStitch.Item2.Item2));
                listOfStitches.RemoveFirst();
                
                List<byte> bytesFromStitch = GetBytesFromStitchCommand(currentStitchCorrectedAxis, sizeOfEachPixel, ref currentPositionX, ref currentPositionY, listOfStitches.Count == 0);
                
                //currentPositionX = currentStitchCorrectedAxis.Item2.Item1;
                //currentPositionY = currentStitchCorrectedAxis.Item2.Item2;

                if (currentPositionX < minX) minX = currentPositionX * sizeOfEachPixel;
                if (currentPositionX > maxX) maxX = currentPositionX * sizeOfEachPixel;
                if (currentPositionY < minY) minY = currentPositionY * sizeOfEachPixel;
                if (currentPositionY > maxY) maxY = currentPositionY * sizeOfEachPixel;

                foreach (byte byteFromStitch in bytesFromStitch)
                {
                    bytesList.Add(byteFromStitch);
                }
            }

            return bytesList;
        }

        private List<byte> GetBytesFromStitchCommand(Tuple<StitchType, Tuple<int, int>> currentStitch, int sizeOfEachPixel, ref int currentPositionX, ref int currentPositionY, bool isLastStitch)
        {
            int maxJumpSize = 121;
            List<byte> bytesFromStitch = new List<byte>();
            int positionToJumpToX, positionToJumpToY;
            switch (currentStitch.Item1)
            {
                case StitchType.ColorChange:
                    if (isLastStitch)
                    {
                        //Jump back to the origin and then do the command 00 00 F3 (Stop command), 

                        //First let's jump back to the origin (0,0)
                        positionToJumpToX = 0;
                        positionToJumpToY = 0;
                        JumpToPositionAndGetBytes(currentStitch, sizeOfEachPixel, ref currentPositionX, ref currentPositionY, maxJumpSize, bytesFromStitch, positionToJumpToX, positionToJumpToY);

                        //The STOP command consists of 00 00 F3
                        bytesFromStitch.Add(0);
                        bytesFromStitch.Add(0);
                        bytesFromStitch.Add((15 << 4) | 3); //this last byte is hexadecimal F3
                    }
                    else
                    {
                        //This is the usual color change command
                        bytesFromStitch.Add(0);
                        bytesFromStitch.Add(0);
                        bytesFromStitch.Add(195);
                    }

                    break;
                case StitchType.JumpStitch:
                    positionToJumpToX = currentStitch.Item2.Item1;
                    positionToJumpToY = currentStitch.Item2.Item2;
                    JumpToPositionAndGetBytes(currentStitch, sizeOfEachPixel, ref currentPositionX, ref currentPositionY, maxJumpSize, bytesFromStitch, positionToJumpToX, positionToJumpToY);
                    break;
                case StitchType.SequinMode:
                    StitchAtPosition(currentStitch, sizeOfEachPixel, ref currentPositionX, ref currentPositionY, bytesFromStitch);
                    break;
                case StitchType.NormalStitch:
                    StitchAtPosition(currentStitch, sizeOfEachPixel, ref currentPositionX, ref currentPositionY, bytesFromStitch);
                    break;
            }

            return bytesFromStitch;
        }

        private void StitchAtPosition(Tuple<StitchType, Tuple<int, int>> currentStitch, int sizeOfEachPixel, ref int currentPositionX, ref int currentPositionY, List<byte> bytesFromStitch)
        {
            int stitchDeltaX = currentStitch.Item2.Item1 - currentPositionX;
            int stitchDeltaY = currentStitch.Item2.Item2 - currentPositionY;
            ConvertCommandToByte(currentStitch.Item1, bytesFromStitch, stitchDeltaX * sizeOfEachPixel, stitchDeltaY * sizeOfEachPixel);
            currentPositionX += stitchDeltaX;
            currentPositionY += stitchDeltaY;
        }

        private void JumpToPositionAndGetBytes(Tuple<StitchType, Tuple<int, int>> currentStitch, int sizeOfEachPixel, ref int currentPositionX, ref int currentPositionY, int maxJumpSize, List<byte> bytesFromStitch, int positionToJumpToX, int positionToJumpToY)
        {
            int jumpSizeSquared = ((positionToJumpToX - currentPositionX) * (positionToJumpToX - currentPositionX) + (positionToJumpToY - currentPositionY) * (positionToJumpToY - currentPositionY)) * sizeOfEachPixel * sizeOfEachPixel;

            int jumpDeltaX, jumpDeltaY;
            if (jumpSizeSquared <= maxJumpSize * maxJumpSize)
            {
                jumpDeltaX = (positionToJumpToX - currentPositionX);
                jumpDeltaY = (positionToJumpToY - currentPositionY);
                ConvertCommandToByte(StitchType.JumpStitch, bytesFromStitch, jumpDeltaX * sizeOfEachPixel, jumpDeltaY * sizeOfEachPixel);
                currentPositionX += jumpDeltaX;
                currentPositionY += jumpDeltaY;
            }
            else
            {
                //TODO: In a sequence of jumps, the current code is wrongfully allowing the last jump to go a distance over 121
                /*int numberOfTimesToJumpBackToOrigin = (int)Math.Ceiling(Math.Sqrt(jumpSizeSquared) / maxJumpSize);

                jumpDeltaX = (positionToJumpToX - currentPositionX) / numberOfTimesToJumpBackToOrigin;
                jumpDeltaY = (positionToJumpToY - currentPositionY) / numberOfTimesToJumpBackToOrigin;

                for (int i = 0; i < numberOfTimesToJumpBackToOrigin; i++)
                {
                    //if (i == numberOfTimesToJumpBackToOrigin - 1)
                    //{
                    //    jumpDeltaX = (positionToJumpToX - currentPositionX);
                    //    jumpDeltaY = (positionToJumpToY - currentPositionY);
                    //}
                    ConvertCommandToByte(StitchType.JumpStitch, bytesFromStitch, jumpDeltaX * sizeOfEachPixel, jumpDeltaY * sizeOfEachPixel);
                    currentPositionX += jumpDeltaX;
                    currentPositionY += jumpDeltaY;
                }

                if(currentPositionX != positionToJumpToX || currentPositionY != positionToJumpToY)
                {
                    jumpDeltaX = (positionToJumpToX - currentPositionX);
                    jumpDeltaY = (positionToJumpToY - currentPositionY);
                    ConvertCommandToByte(StitchType.JumpStitch, bytesFromStitch, jumpDeltaX * sizeOfEachPixel, jumpDeltaY * sizeOfEachPixel);
                    currentPositionX += jumpDeltaX;
                    currentPositionY += jumpDeltaY;
                }*/

                while (currentPositionX != positionToJumpToX || currentPositionY != positionToJumpToY)
                {
                    jumpDeltaX = (positionToJumpToX - currentPositionX);
                    jumpDeltaY = (positionToJumpToY - currentPositionY);

                    jumpSizeSquared = (jumpDeltaX * jumpDeltaX + jumpDeltaY * jumpDeltaY);

                    if (jumpSizeSquared * sizeOfEachPixel * sizeOfEachPixel > maxJumpSize * maxJumpSize)
                    {
                        jumpDeltaX = (int)((((float)jumpDeltaX) / Math.Sqrt(jumpSizeSquared)) * (((float)maxJumpSize) / sizeOfEachPixel));
                        jumpDeltaY = (int)((((float)jumpDeltaY) / Math.Sqrt(jumpSizeSquared)) * (((float)maxJumpSize) / sizeOfEachPixel));
                    }

                    ConvertCommandToByte(StitchType.JumpStitch, bytesFromStitch, jumpDeltaX * sizeOfEachPixel, jumpDeltaY * sizeOfEachPixel);
                    currentPositionX += jumpDeltaX;
                    currentPositionY += jumpDeltaY;
                }
            }
        }

        private void ConvertCommandToByte(StitchType stitchType, List<byte> bytesFromStitch, int amountToMoveX, int amountToMoveY)
        {           
            List<int> movePowersX = ConvertNumberToPowersOfNumber(amountToMoveX, 3);
            List<int> movePowersY = ConvertNumberToPowersOfNumber(amountToMoveY, 3);

            //completing the powers with zeros up to 81=3^5
            while (movePowersX.Count < 5)
            {
                movePowersX.Add(0);
            }
            while (movePowersY.Count < 5)
            {
                movePowersY.Add(0);
            }
            byte[] bytesFromPowersAndCommand = GetBytesFromPowersAndCommand(movePowersX, movePowersY, stitchType);
            for (int i = 0; i < 3; i++)
            {
                bytesFromStitch.Add(bytesFromPowersAndCommand[i]);
            }
        }

        private byte[] GetBytesFromPowersAndCommand(List<int> jumpPowersX, List<int> jumpPowersY, StitchType stitchType)
        {
            //The structure of this bytes follows a very particular pattern specific to the .dst format

            byte[] commandBytes = new byte[3];

            commandBytes[0] = (byte)(jumpPowersY[0] >= 0 ? jumpPowersY[0] << 7 : 1 << 6);
            commandBytes[0] |= (byte)(jumpPowersY[2] >= 0 ? jumpPowersY[2] << 5 : 1 << 4);
            commandBytes[0] |= (byte)(jumpPowersX[2] >= 0 ? jumpPowersX[2] << 2 : 1 << 3);
            commandBytes[0] |= (byte)(jumpPowersX[0] >= 0 ? jumpPowersX[0] << 0 : 1 << 1);  // "<< 0" does nothing but I wrote it this way to keep the standard of the other parts so I can easily know what the code is doing when reading

            commandBytes[1] = (byte)(jumpPowersY[1] >= 0 ? jumpPowersY[1] << 7 : 1 << 6);
            commandBytes[1] |= (byte)(jumpPowersY[3] >= 0 ? jumpPowersY[3] << 5 : 1 << 4);
            commandBytes[1] |= (byte)(jumpPowersX[3] >= 0 ? jumpPowersX[3] << 2 : 1 << 3);
            commandBytes[1] |= (byte)(jumpPowersX[1] >= 0 ? jumpPowersX[1] << 0 : 1 << 1);

            //The two first bits of the third byte are control bits, which tell the type of stitch that the machine needs to perform
            switch (stitchType)
            {
                case StitchType.NormalStitch:
                    commandBytes[2] = 0;
                    break;
                case StitchType.JumpStitch:
                    commandBytes[2] = (byte)1 << 7;
                    break;
                case StitchType.ColorChange:
                    commandBytes[2] = (byte)3 << 6;
                    break;
                case StitchType.SequinMode:
                    commandBytes[2] = (byte)1 << 6;
                    break;
            }
            commandBytes[2] |= (byte)(jumpPowersY[4] >= 0 ? jumpPowersY[4] << 5 : 1 << 4);
            commandBytes[2] |= (byte)(jumpPowersX[4] >= 0 ? jumpPowersX[4] << 2 : 1 << 3);
            commandBytes[2] |= (byte)3; //two last bits are always set in the .dst format            

            return commandBytes;
        }

        private List<byte> GetEncodingDstFileHeader(List<byte> bodyEncoding, int minX, int maxX, int minY, int maxY)
        {
            //DstHeaderInformation dstHeaderInformation = new DstHeaderInformation();

            string label = "LA:" + "fileName";
            while (label.Length < 3 + 16)
            {
                label += " ";
            }
            //label += ".";

            string stitches = "ST:";
            stitches = FillValueAndAddPaddingToHeaderField(stitches, 7, (bodyEncoding.Count / 3).ToString());

            int numberOfColors = 0;
            for (int i = 2; i < bodyEncoding.Count; i += 3)
            {
                if ((bodyEncoding[i] | (195)) != 0)
                {
                    numberOfColors++;
                }
            }
            string numberOfColorsString = "CO:";
            numberOfColorsString = FillValueAndAddPaddingToHeaderField(numberOfColorsString, 3, numberOfColors.ToString());

            string xPlusExtends = "+X:";
            xPlusExtends = FillValueAndAddPaddingToHeaderField(xPlusExtends, 5, maxX.ToString());

            string xMinusExtends = "-X:";
            xMinusExtends = FillValueAndAddPaddingToHeaderField(xMinusExtends, 5, minX.ToString());

            string yPlusExtends = "+Y:";
            yPlusExtends = FillValueAndAddPaddingToHeaderField(yPlusExtends, 5, maxY.ToString());

            string yMinusExtends = "-Y:";
            yMinusExtends = FillValueAndAddPaddingToHeaderField(yMinusExtends, 5, minY.ToString());

            string aX = "AX:";
            aX = FillValueAndAddPaddingToHeaderField(aX, 6, "0");

            string aY = "AY:";
            aY = FillValueAndAddPaddingToHeaderField(aY, 6, "0");

            string mX = "MX:";
            mX = FillValueAndAddPaddingToHeaderField(mX, 6, "0");

            string mY = "MY:";
            mY = FillValueAndAddPaddingToHeaderField(mY, 6, "0");

            string pD = "PD:******";
            //pD += ".";

            byte[] labelBytes = Encoding.ASCII.GetBytes(label);
            byte[] stitchesBytes = Encoding.ASCII.GetBytes(stitches);
            byte[] numberOfColorsBytes = Encoding.ASCII.GetBytes(numberOfColorsString);
            byte[] xPlusExtendsBytes = Encoding.ASCII.GetBytes(xPlusExtends);
            byte[] xMinusExtendsBytes = Encoding.ASCII.GetBytes(xMinusExtends);
            byte[] yPlusExtendsBytes = Encoding.ASCII.GetBytes(yPlusExtends);
            byte[] yMinusExtendsBytes = Encoding.ASCII.GetBytes(yMinusExtends);
            byte[] aXBytes = Encoding.ASCII.GetBytes(aX);
            byte[] aYBytes = Encoding.ASCII.GetBytes(aY);
            byte[] mXBytes = Encoding.ASCII.GetBytes(mX);
            byte[] mYBytes = Encoding.ASCII.GetBytes(mY);
            byte[] pDBytes = Encoding.ASCII.GetBytes(pD);

            List<Byte> allHeaderBytes = new List<byte>();
            FillBytesAndCarriageReturn(labelBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(stitchesBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(numberOfColorsBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(xPlusExtendsBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(xMinusExtendsBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(yPlusExtendsBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(yMinusExtendsBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(aXBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(aYBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(mXBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(mYBytes, allHeaderBytes);
            FillBytesAndCarriageReturn(pDBytes, allHeaderBytes);

            allHeaderBytes.Add(26); //This byte 1A (hexadecimal) indicates the end of the header

            //Now complete by adding the padding in order to have a total of 512 bytes in the header
            while (allHeaderBytes.Count < 512)
            {
                allHeaderBytes.Add(Encoding.ASCII.GetBytes(" ")[0]);
            }

            return allHeaderBytes;
        }

        private static void FillBytesAndCarriageReturn(byte[] labelBytes, List<byte> allHeaderBytes)
        {
            foreach (byte labelByte in labelBytes)
            {
                allHeaderBytes.Add(labelByte);
            }
            allHeaderBytes.Add(13); //0D (hexadecimal) at the end of each field
        }

        private static string FillValueAndAddPaddingToHeaderField(string headerField, int sizeOfThisField, string headerFieldValue)
        {
            int initialSize = headerField.Length;
            while (headerField.Length + headerFieldValue.Length < initialSize + sizeOfThisField)
            {
                headerField += " "; //padding of 20 (hexadecimal)
            }
            headerField += headerFieldValue;
            //headerField += ".";

            return headerField;
        }

        private List<int> ConvertNumberToPowersOfNumber(int number, int basis = 3)
        {
            int absoluteValue = Math.Abs(number);

            List<int> powers = new List<int>();

            if (absoluteValue == 1 || absoluteValue == 0)
            {
                powers.Add(number);
                return powers;
            }

            //Let's place the number between two powers of basis
            int upperExponent = (int)Math.Ceiling(Math.Log(absoluteValue, basis));
            int lesserExponent = upperExponent - 1;

            if(absoluteValue >= Math.Pow(basis, upperExponent) / (basis - 1))
            {
                //then upper exponent does appear in the original number's representation by exponents
                powers = ConvertNumberToPowersOfNumber((int)Math.Pow(basis, upperExponent) - absoluteValue, basis);
                for (int i = 0; i < powers.Count; i++)
                {
                    powers[i] *= -1;
                }

                while(powers.Count < upperExponent)
                {
                    powers.Add(0);
                }
                powers.Add(1);
            }
            else
            {
                //In this case, it's rather the lesser exponent that appears in the original number's representation by exponents and not upper exponent
                powers = ConvertNumberToPowersOfNumber(absoluteValue - (int)Math.Pow(basis, lesserExponent), basis);

                while (powers.Count < lesserExponent)
                {
                    powers.Add(0);
                }
                powers.Add(1);
            }

            if (Math.Sign(number) < 0)
            {
                //invert all the signs
                for (int i = 0; i < powers.Count; i++)
                {
                    powers[i] *= -1;
                }
            }

            return powers;
        }

        public LinkedList<Tuple<StitchType, Tuple<int, int>>> CreatePath(Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, int horizontalSize, int verticalSize)
        {
            LinkedList<Tuple<StitchType, Tuple<int, int>>> listOfStitches = new LinkedList<Tuple<StitchType, Tuple<int, int>>>();

            //int numberOfColorsAlreadyUsed = 0;
            //foreach (int colorIndex in positionsOfEachColor.Keys)
            //{
            //    //Create path starting by right diagonal
            //    CreateStitchForColorPathStartingByCertainTypeOfDiagonal(positionsOfEachColor[colorIndex], listOfStitches, true);

            //    //Create path starting by left diagonal
            //    CreateStitchForColorPathStartingByCertainTypeOfDiagonal(positionsOfEachColor[colorIndex], listOfStitches, false);                

            //    numberOfColorsAlreadyUsed++;
            //    if (numberOfColorsAlreadyUsed < positionsOfEachColor.Keys.Count)
            //    {
            //        listOfStitches.Add(new Tuple<StitchType, Tuple<int, int>>(StitchType.ColorChange, new Tuple<int, int>(0, 0)));
            //    }
            //}

            //All .dst files I looked at start with two jumps of (0,0)
            listOfStitches.AddLast(new Tuple<StitchType, Tuple<int, int>>(StitchType.JumpStitch, new Tuple<int, int>(0, 0)));
            listOfStitches.AddLast(new Tuple<StitchType, Tuple<int, int>>(StitchType.JumpStitch, new Tuple<int, int>(0, 0)));

            for (int i = 0; i < positionsOfEachColor.Keys.Count; i++)
            {
                //Create path starting by right diagonal
                CreateStitchForColorPathStartingByCertainTypeOfDiagonal(positionsOfEachColor[positionsOfEachColor.Keys.ElementAt(i)], listOfStitches, true, horizontalSize, verticalSize);

                //Create path starting by left diagonal
                CreateStitchForColorPathStartingByCertainTypeOfDiagonal(positionsOfEachColor[positionsOfEachColor.Keys.ElementAt(i)], listOfStitches, false, horizontalSize, verticalSize);

                listOfStitches.AddLast(new Tuple<StitchType, Tuple<int, int>>(StitchType.ColorChange, new Tuple<int, int>(0, 0)));
                //At the moment of converting the embroidery path to machine file, when placing the last color change, it needs to be specifically the command 00 00 F3
            }

            return listOfStitches;
        }

        private void CreateStitchForColorPathStartingByCertainTypeOfDiagonal(List<Tuple<int, int>> positionsForTheSpecifiedColor, LinkedList<Tuple<StitchType, Tuple<int, int>>> listOfStitches, bool startsAtEvenDiagonalParity, int horizontalSize, int verticalSize)
        {
            //Create dictionary/hashset of positions of each color
            HashSet<Tuple<int, int>> allPositionsForCurrentColor = new HashSet<Tuple<int, int>>();//positionsForTheSpecifiedColor.ToHashSet<Tuple<int, int>>();
            int halfHorizontalSize = (int)(horizontalSize * 0.5);
            int halfVerticalSize = (int)(verticalSize * 0.5);
            foreach (Tuple<int, int> position in positionsForTheSpecifiedColor)
            {
                allPositionsForCurrentColor.Add(new Tuple<int, int>(position.Item1 - halfHorizontalSize, position.Item2 - halfVerticalSize));
            }

            //While the dictionary created on the previous step is not empty, perform loop containing the next two steps
            while (allPositionsForCurrentColor.Count > 0)
            {
                //Find next connected region for this color starting with the correspondent type diagonal, be it right or left diagonal (Flood Fill algorithm)
                //Don't forget to remove each position from the dictionary throughout the flood fill algorithm
                Tuple<int, int> startingPosition = allPositionsForCurrentColor.First<Tuple<int, int>>();
                int startingPositionParity = startingPosition.Item1 + startingPosition.Item2;
                bool startsAtRightDiagonal = (startingPositionParity % 2 == 0) ^ !startsAtEvenDiagonalParity;
                HashSet<Tuple<int, int>> currentConnectedRegion = FloodFill(allPositionsForCurrentColor, startsAtRightDiagonal);

                //Create path for the connected region starting with the correspondent type diagonal, be it right or left diagonal, found in previous step (Chinese Postman Problem)
                LinkedList<Tuple<int, int>> pathToFollow = CreateShortestPath(currentConnectedRegion, startsAtRightDiagonal);

                //bool alreadyJumpedToThisNextPath = false;
                listOfStitches.AddLast(new Tuple<StitchType, Tuple<int, int>>(StitchType.JumpStitch, pathToFollow.First()));
                foreach (Tuple<int, int> node in pathToFollow)
                {
                    //if (!alreadyJumpedToThisNextPath)
                    //{
                    //    listOfStitches.Add(new Tuple<StitchType, Tuple<int, int>>(StitchType.JumpStitch, node));
                    //    alreadyJumpedToThisNextPath = true;
                    //}
                    listOfStitches.AddLast(new Tuple<StitchType, Tuple<int, int>>(StitchType.NormalStitch, node));
                }
            }
        }

        private LinkedList<Tuple<int, int>> CreateShortestPath(HashSet<Tuple<int, int>> currentConnectedRegion, bool startsAtRightDiagonal)
        {
            //Prepare for the implementation by first creating both vertices and edges
            //For vertices I will use the coordinates of the top left corner of each square as the way of identifying each vertex / associating coordinates to them
            HashSet<Tuple<int, int>> vertices = new HashSet<Tuple<int, int>>();
            HashSet<Edge> edges = new HashSet<Edge>();

            Tuple<int, int> startingPosition = currentConnectedRegion.First<Tuple<int, int>>();

            foreach(Tuple<int, int> currentPosition in currentConnectedRegion)
            {
                //For each position add two vertices and one edge
                bool isRightDiagonal = CheckIfDiagonalIsRightOrLeft(startsAtRightDiagonal, startingPosition, currentPosition);

                Tuple<int, int> upperVertex, bottomVertex;
                if (isRightDiagonal)
                {
                    //Upper right vertex
                    upperVertex = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2);
                    //Bottom left vertex
                    bottomVertex = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1);
                }
                else
                {
                    //Upper left vertex
                    upperVertex = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2);
                    //Bottom right vertex
                    bottomVertex = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 + 1);
                }
                vertices.Add(upperVertex);
                vertices.Add(bottomVertex);
                edges.Add(new Edge(upperVertex, bottomVertex));
            }

            //Compute degree of each vertex
            Dictionary<Tuple<int, int>, int> degreeOfEachVertex = new Dictionary<Tuple<int, int>, int>();
            foreach(Edge edge in edges)
            {
                UpdateDegreeOfVertex(degreeOfEachVertex, edge.upperVertex);
                UpdateDegreeOfVertex(degreeOfEachVertex, edge.bottomVertex);
            }

            HashSet<Tuple<int, int>> oddDegreeVertices = new HashSet<Tuple<int, int>>();
            foreach(KeyValuePair<Tuple<int, int>, int> vertexWithDegree in degreeOfEachVertex)
            {
                if(vertexWithDegree.Value % 2 != 0)
                {
                    oddDegreeVertices.Add(vertexWithDegree.Key);
                }
            }

            //Duplicate edges relative to odd degree vertices
            //carefull when duplicating edges because of odd degree vertices, the edges variable used so far is a HashSet, which doesn't accept duplication of values
            //I might use a dictionary containing the edge as key and the number of times that edge is present in the graph as the value
            Dictionary<Edge, int> edgesAndNumberOfTimesItAppears = new Dictionary<Edge, int>();
            foreach(Edge edge in edges)
            {
                edgesAndNumberOfTimesItAppears.Add(edge, 1);
            }
            DuplicateEdgesRelativeToOddDegreeVertices(oddDegreeVertices, edgesAndNumberOfTimesItAppears);

            //Find Eulerian cycle now that the graph is guaranteed to be Eulerian
            Tuple<int, int> startingPositionForEulerianCycle = vertices.First<Tuple<int, int>>();
            LinkedList<Tuple<int, int>> eulerianCycle = MakeEulerianCycle(vertices, edgesAndNumberOfTimesItAppears, startingPositionForEulerianCycle);

            return eulerianCycle;
        }

        private LinkedList<Tuple<int, int>> MakeEulerianCycle(HashSet<Tuple<int, int>> vertices, Dictionary<Edge, int> edgesAndNumberOfTimesItAppears, Tuple<int, int> startingPosition)
        {
            LinkedList<Tuple<int, int>> tour = MakeTour(vertices, edgesAndNumberOfTimesItAppears, startingPosition);

            //while there are vertices in this tour that have unused edges, call this very function recursively in order to make new tours starting on each of those vertices and returning to it
            //var startingPositionIn = MakeEulerianCycle(vertices, edgesAndNumberOfTimesItAppears)
            //Tuple<int, int> currentPosition = startingPosition;
            LinkedListNode<Tuple<int, int>> currentNode = tour.First;
            
            if (tour.Count <= 1) return tour;

            do
            {
                LinkedList<Tuple<int, int>> localTour;
                do
                {
                    localTour = MakeEulerianCycle(vertices, edgesAndNumberOfTimesItAppears, /*currentPosition*/currentNode.Value);
                    //Incorporate this local tour to the global tour
                    LinkedListNode<Tuple<int, int>> lastAddedNode = currentNode;
                    LinkedListNode<Tuple<int, int>> nextNodeAfterLocalTour = currentNode.Next;
                    foreach (Tuple<int, int> newNode in localTour)
                    {
                        lastAddedNode =  tour.AddBefore(nextNodeAfterLocalTour, newNode);
                    }

                    //Take that new tour and connect properly to the tour the vertex belonged to
                    tour.Remove(currentNode);
                    currentNode = lastAddedNode;
                    //currentPosition = currentNode.Value;

                        

                } while (localTour.Count > 1);
                //move current node to next node in the sequence
                currentNode = currentNode.Next;
                //currentPosition = currentNode.Value;

            } while (/*currentPosition*/!currentNode.Value.Equals(startingPosition));



            //Return the path resulting after the while loop

            return tour;
        }

        private LinkedList<Tuple<int, int>> MakeTour(HashSet<Tuple<int, int>> vertices, Dictionary<Edge, int> edgesAndNumberOfTimesItAppears, Tuple<int, int> startingPosition)
        {
            Tuple<int, int> currentPosition = startingPosition;

            LinkedList<Tuple<int, int>> tour = new LinkedList<Tuple<int, int>>();

            do
            {
                //Adding the current position to our tour
                tour.AddLast(currentPosition);
                List<Edge> unusedEdgesConnectedToThisVertex;
                List<Tuple<int, int>> verticesConnectedToThisVertex;
                GetVerticesAndEdgesConnectedToThisVertex(edgesAndNumberOfTimesItAppears, currentPosition, out unusedEdgesConnectedToThisVertex, out verticesConnectedToThisVertex);

                //Randomly choosing one of the vertices connected to the current vertex and moving to there
                if(verticesConnectedToThisVertex.Count > 0)
                {
                    Random rnd = new Random();
                    int nextPositionIndex = rnd.Next(0, verticesConnectedToThisVertex.Count);
                    Tuple<int, int> nextPosition = verticesConnectedToThisVertex[nextPositionIndex];
                    edgesAndNumberOfTimesItAppears[unusedEdgesConnectedToThisVertex[nextPositionIndex]]--;

                    currentPosition = nextPosition;
                }

            } while (!currentPosition.Equals(startingPosition));

            if(tour.Count > 1)
            {
                tour.AddLast(startingPosition); //Adding the starting position again to complete a closed tour, but only if the tour has more elements besides the starting one
            }

            return tour;
        }

        private static void GetVerticesAndEdgesConnectedToThisVertex(Dictionary<Edge, int> edgesAndNumberOfTimesItAppears, Tuple<int, int> currentPosition, out List<Edge> unusedEdgesConnectedToThisVertex, out List<Tuple<int, int>> verticesConnectedToThisVertex)
        {
            //Add connecting vertices
            //Upper left
            Tuple<int, int> upperLeftPosition = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2 - 1);
            Edge upperLeftEdge = new Edge(upperLeftPosition, currentPosition);
            //Upper right
            Tuple<int, int> upperRightPosition = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 - 1);
            Edge upperRightEdge = new Edge(upperRightPosition, currentPosition);
            //Bottom left
            Tuple<int, int> bottomLeftPosition = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2 + 1);
            Edge bottomLeftEdge = new Edge(currentPosition, bottomLeftPosition);
            //Bottom right
            Tuple<int, int> bottomRightPosition = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 + 1);
            Edge bottomRightEdge = new Edge(currentPosition, bottomRightPosition);

            unusedEdgesConnectedToThisVertex = new List<Edge>();
            verticesConnectedToThisVertex = new List<Tuple<int, int>>();
            if (edgesAndNumberOfTimesItAppears.ContainsKey(upperLeftEdge) && edgesAndNumberOfTimesItAppears[upperLeftEdge] > 0)
            {
                unusedEdgesConnectedToThisVertex.Add(upperLeftEdge);
                verticesConnectedToThisVertex.Add(upperLeftPosition);
            }
            if (edgesAndNumberOfTimesItAppears.ContainsKey(upperRightEdge) && edgesAndNumberOfTimesItAppears[upperRightEdge] > 0)
            {
                unusedEdgesConnectedToThisVertex.Add(upperRightEdge);
                verticesConnectedToThisVertex.Add(upperRightPosition);
            }
            if (edgesAndNumberOfTimesItAppears.ContainsKey(bottomLeftEdge) && edgesAndNumberOfTimesItAppears[bottomLeftEdge] > 0)
            {
                unusedEdgesConnectedToThisVertex.Add(bottomLeftEdge);
                verticesConnectedToThisVertex.Add(bottomLeftPosition);
            }
            if (edgesAndNumberOfTimesItAppears.ContainsKey(bottomRightEdge) && edgesAndNumberOfTimesItAppears[bottomRightEdge] > 0)
            {
                unusedEdgesConnectedToThisVertex.Add(bottomRightEdge);
                verticesConnectedToThisVertex.Add(bottomRightPosition);
            }
        }

        private void DuplicateEdgesRelativeToOddDegreeVertices(HashSet<Tuple<int, int>> oddDegreeVertices, Dictionary<Edge, int> edgesAndNumberOfTimesItAppears)
        {
            //Breadth first search while both keeping track of already visited vertices and "parent" of each vertex, i.e., the vertex that came before the current vertex in that path
            
            while(oddDegreeVertices.Count > 0)
            {
                HashSet<Tuple<int, int>> alreadyVisitedVertices = new HashSet<Tuple<int, int>>();

                Tuple<int, int> startingPosition = oddDegreeVertices.First<Tuple<int, int>>();
                VertexAndParent startingVertex = new VertexAndParent(startingPosition, null);

                Queue<VertexAndParent> queue = new Queue<VertexAndParent>();
                HashSet<Tuple<int, int>> alreadyEnqueued = new HashSet<Tuple<int, int>>();

                queue.Enqueue(startingVertex);
                alreadyEnqueued.Add(startingVertex.vertex);

                VertexAndParent endingVertex = startingVertex;
                //Breadth First Search from the top position among the odd degree vertices until I find another vertex that also is an odd degree vertex
                while (queue.Count > 0)
                {
                    VertexAndParent currentVertex = queue.Dequeue();
                    alreadyVisitedVertices.Add(currentVertex.vertex);
                    
                    if (oddDegreeVertices.Contains(currentVertex.vertex) && !currentVertex.vertex.Equals(startingPosition))
                    {
                        endingVertex = currentVertex;
                        break;
                    }

                    //Add connecting vertices
                    //Upper left
                    Tuple<int, int> upperLeftPosition = new Tuple<int, int>(currentVertex.vertex.Item1 - 1, currentVertex.vertex.Item2 - 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, upperLeftPosition, false, alreadyEnqueued);
                    //Upper right
                    Tuple<int, int> upperRightPosition = new Tuple<int, int>(currentVertex.vertex.Item1 + 1, currentVertex.vertex.Item2 - 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, upperRightPosition, false, alreadyEnqueued);
                    //Bottom left
                    Tuple<int, int> bottomLeftPosition = new Tuple<int, int>(currentVertex.vertex.Item1 - 1, currentVertex.vertex.Item2 + 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, bottomLeftPosition, true, alreadyEnqueued);
                    //Bottom right
                    Tuple<int, int> bottomRightPosition = new Tuple<int, int>(currentVertex.vertex.Item1 + 1, currentVertex.vertex.Item2 + 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, bottomRightPosition, true, alreadyEnqueued);
                }

                //Double all edges going from the ending vertex to the starting vertex
                //Back tracing from ending vertex until the parent vertex is null
                oddDegreeVertices.Remove(endingVertex.vertex);
                while(endingVertex.parent != null)
                {
                    Edge edgeToDouble;
                    if (endingVertex.parent.vertex.Item2 < endingVertex.vertex.Item2)
                    {
                        //Parent vertex is above the current vertex, the parent vertex then needs to come first in the edge as per the convention used in this code
                        edgeToDouble = new Edge(endingVertex.parent.vertex, endingVertex.vertex);
                    }
                    else
                    {
                        //Parent vertex is below the current vertex, the parent vertex then needs to come after the current one in the edge as per the convention used in this code
                        edgeToDouble = new Edge(endingVertex.vertex, endingVertex.parent.vertex);
                    }
                    //"Double" this edge
                    edgesAndNumberOfTimesItAppears[edgeToDouble]++;
                    endingVertex = endingVertex.parent;
                }
                oddDegreeVertices.Remove(startingPosition);
            }
        }

        private static void TryToEnqueueNewVertex(Dictionary<Edge, int> edgesAndNumberOfTimesItAppears, HashSet<Tuple<int, int>> alreadyVisitedVertices, Queue<VertexAndParent> queue, VertexAndParent currentVertex, Tuple<int, int> potentialNewPosition, bool currentVertexIsUpper, HashSet<Tuple<int, int>> alreadyEnqueued)
        {
            if (!alreadyVisitedVertices.Contains(potentialNewPosition))
            {
                Edge potentialEdge = currentVertexIsUpper ? new Edge(currentVertex.vertex, potentialNewPosition) : new Edge(potentialNewPosition, currentVertex.vertex);
                VertexAndParent newVertex = new VertexAndParent(potentialNewPosition, currentVertex);
                if (edgesAndNumberOfTimesItAppears.ContainsKey(potentialEdge) && !alreadyEnqueued.Contains(newVertex.vertex))
                {
                    queue.Enqueue(newVertex);
                    alreadyEnqueued.Add(newVertex.vertex);
                }
            }
        }

        private static void UpdateDegreeOfVertex(Dictionary<Tuple<int, int>, int> degreeOfEachVertex, Tuple<int, int> vertexToUpdate)
        {
            if (!degreeOfEachVertex.ContainsKey(vertexToUpdate))
            {
                degreeOfEachVertex.Add(vertexToUpdate, 1);
            }
            else
            {
                degreeOfEachVertex[vertexToUpdate]++;
            }
        }

        private bool CheckIfDiagonalIsRightOrLeft(bool startsAtRightDiagonal, Tuple<int, int> startingPosition, Tuple<int, int> currentPosition)
        {
            int startingPositionParity = startingPosition.Item1 + startingPosition.Item2;
            int currentPositionParity = currentPosition.Item1 + currentPosition.Item2;

            int parityDifference = currentPositionParity - startingPositionParity;
            bool currentPositionIsSameTypeOfDiagonalThanStartingPosition = parityDifference % 2 == 0;
            //bool currentPositionIsRightDiagonal = startsAtRightDiagonal ? currentPositionIsSameTypeOfDiagonalThanStartingPosition : !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
            bool currentPositionIsRightDiagonal = startsAtRightDiagonal ^ !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
            return currentPositionIsRightDiagonal;
        }

        private HashSet<Tuple<int, int>> FloodFill(HashSet<Tuple<int, int>> allPositionsForCurrentColor, bool startsAtRightDiagonal)
        {
            //This version of the Flood Fill algorithm works at diagonal because that's how the path the thread takes in cross stitching

            HashSet<Tuple<int, int>> connectedRegion = new HashSet<Tuple<int, int>>();

            Tuple<int, int> startingPosition = allPositionsForCurrentColor.First<Tuple<int, int>>();
            int startingPositionParity = startingPosition.Item1 + startingPosition.Item2;

            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(startingPosition);

            while(queue.Count > 0)
            {
                Tuple<int, int> currentPosition = queue.Dequeue();

                connectedRegion.Add(currentPosition);
                allPositionsForCurrentColor.Remove(currentPosition);

                //Try to add adjacent positions to the queue
                //int currentPositionParity = currentPosition.Item1 + currentPosition.Item2;
                //int parityDifference = currentPositionParity - startingPositionParity;
                //bool currentPositionIsSameTypeOfDiagonalThanStartingPosition = parityDifference % 2 == 0;
                //bool currentPositionIsRightDiagonal = startsAtRightDiagonal ? currentPositionIsSameTypeOfDiagonalThanStartingPosition : !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
                //bool currentPositionIsRightDiagonal = startsAtRightDiagonal ^ !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
                bool currentPositionIsRightDiagonal = CheckIfDiagonalIsRightOrLeft(startsAtRightDiagonal, startingPosition, currentPosition);

                List<Tuple<int, int>> adjacentPositionsToFlood = new List<Tuple<int, int>>();
                for (int i = currentPosition.Item1 - 1; i <= currentPosition.Item1 + 1; i++)
                {
                    for (int j = currentPosition.Item2 - 1; j <= currentPosition.Item2 + 1; j++)
                    {
                        adjacentPositionsToFlood.Add(new Tuple<int, int>(i, j));
                    }
                }

                adjacentPositionsToFlood.Remove(currentPosition);
                if (currentPositionIsRightDiagonal)
                {
                    adjacentPositionsToFlood.Remove(new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2 - 1));
                    adjacentPositionsToFlood.Remove(new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 + 1));
                }
                else
                {
                    adjacentPositionsToFlood.Remove(new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 - 1));
                    adjacentPositionsToFlood.Remove(new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2 + 1));
                }

                foreach(var positionToFlood in adjacentPositionsToFlood)
                {
                    CheckPositionAndTryToEnqueue(allPositionsForCurrentColor, connectedRegion, queue, positionToFlood);
                }
            }

            return connectedRegion;
        }

        private static void CheckPositionAndTryToEnqueue(HashSet<Tuple<int, int>> allPositionsForCurrentColor, HashSet<Tuple<int, int>> connectedRegion, Queue<Tuple<int, int>> queue, Tuple<int, int> positionToEnqueue)
        {
            if (allPositionsForCurrentColor.Contains(positionToEnqueue) && !connectedRegion.Contains(positionToEnqueue) && !queue.Contains(positionToEnqueue))
            {
                queue.Enqueue(positionToEnqueue);
            }
        }
    }


    enum StitchType
    {
        NormalStitch, 
        JumpStitch,
        ColorChange,
        SequinMode
    }

    public struct Edge
    {
        public Tuple<int, int> upperVertex;
        public Tuple<int, int> bottomVertex;

        public Edge(Tuple<int, int> upperVertex, Tuple<int, int> bottomVertex)
        {
            this.upperVertex = upperVertex;
            this.bottomVertex = bottomVertex;
        }
    }

    public class VertexAndParent
    {
        public Tuple<int, int> vertex;
        public VertexAndParent parent;

        public VertexAndParent(Tuple<int, int> vertex, VertexAndParent parent)
        {
            this.vertex = vertex;
            this.parent = parent;
        }
    }

    public class DstHeaderInformation
    {
        public char[] label = new char[16];
        public char[] stitches = new char[7];
        public char[] colors = new char[3];
        public char[] xPlusExtends = new char[5];
        public char[] xMinusExtends = new char[5];
        public char[] yPlusExtends = new char[5];
        public char[] yMinusExtends = new char[5];
        public char[] aX = new char[6];
        public char[] aY = new char[6];
        public char[] mX = new char[6];
        public char[] mY = new char[6];
        public char[] pD = new char[9];
    }
}


//TODO: Start the file with 2 jumps of (0,0), then jump once more to the position where I want to embroider and start by making a normal stitch there of (0,0)
//TODO: The stitches use a difference of movement between the previous position and the next one. In most of this program I was using the absolute positions of the stitches
//TODO: DST files have a maximum stitch/jump length of 121
//TODO: At the end after having made all stitches, jump back to the position (0,0). Note that this is different than a command of jump (0,0), which would mean to jump a relative position of (0,0),
//which would mean not moving at all.
//TODO: Maybe end the embroidery file with 3 jump stitches, the first one moves 1 to the right, the second one moves one back to the left (thus going nowhere) and the third one coming back to the position (0,0)
//TODO: Or maybe end the file with two jumps (the second one going back to the origin (0,0) ) and a color change command
//TODO: Except from the three initial and the three last jumps, separate long jumps into jumps plus one stitch to the desired position. Ex.: If for example the needle is at the position (1,2)
//and I want to jump to the position (16,20), separate it into two jumps of (5,6) and more a normal stitch of (5,6); these three movements will end up with the total of relative movement of (15,18)
//TODO: When we have only one jump (in order to jump to a separate position not too far away, use a jump that takes the needle to that position and the use a normal stitch of (0,0) (relative movement
//of (0,0) )
//TODO: When doing a color change and a jump, one right after the other, do first the color change and do the jump next
//TODO: The size of the stitches isn't something predefined, it's upon to the instructions given by the file, thus the stitch size is bound to be different from one file to another
//TODO: The last color change does add up indeed to the number of colors in an embroidery

/* roses -> 15 jumps back to origin, then color change 00 00 F3
 * fruits -> 2 jumps back to origin, then color change 00 00 F3
 * chicken -> 7 jumps back to origin, then color change 00 00 F3
 * bumble_bee -> 3 jumps back to origin, then color change 00 00 F3
 * cat -> no jump, the design ends up in a different position than the origin, then color change
 */