﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration.ObjectChunks.ObjectLands
{
    public static class LandCreationHelper
    {
        private static int[,] CENTER_MATRIX = new int[,]
        {
            {1, 2, 1},
            {2, 0, 2},
            {1, 2, 1}
        };

        private static int[,] HORIZONTAL_MATRIX = new int[,]
        {
            {2, 2, 2},
            {1, 0, 1},
            {2, 2, 2}
        };

        private static int[,] VERTICAL_MATRIX = new int[,]
        {
            {2, 1, 2},
            {2, 0, 2},
            {2, 1, 2}
        };

        private static int[,] BEND_BOT_LEFT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {0, 0, 1},
            {1, 0, 2}
        };

        private static int[,] BEND_TOP_LEFT_MATRIX = new int[,]
        {
            {1, 0, 2},
            {0, 0, 1},
            {2, 1, 2}
        };

        private static int[,] BEND_TOP_RIGHT_MATRIX = new int[,]
        {
            {2, 0, 1},
            {1, 0, 0},
            {2, 1, 2}
        };

        private static int[,] BEND_BOT_RIGHT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {1, 0, 0},
            {2, 0, 1}
        };

        //
        private static int[,] BEND_WATER_MATRIX = new int[,]
        {
            {2, 0, 2},
            {0, 1, 0},
            {2, 0, 2}
        };

        ///----------------------------------///

        private static int[,] FULL_MATRIX = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };

        private static int[,] RIGHT_MATRIX = new int[,]
        {
            {2, 0, 0},
            {1, 0, 0},
            {2, 0, 0}
        };

        private static int[,] BOT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {0, 0, 0},
            {0, 0, 0}
        };

        private static int[,] LEFT_MATRIX = new int[,]
        {
            {0, 0, 2},
            {0, 0, 1},
            {0, 0, 2}
        };

        private static int[,] TOP_MATRIX = new int[,]
        {
            {0, 0, 0},
            {0, 0, 0},
            {2, 1, 2}
        };

        //
        private static int[,] TOP_RIGHT_MATRIX = new int[,]
        {
            {0, 0, 0},
            {0, 0, 0},
            {1, 0, 0}
        };

        private static int[,] BOT_RIGHT_MATRIX = new int[,]
        {
            {1, 0, 0},
            {0, 0, 0},
            {0, 0, 0}
        };

        private static int[,] BOT_LEFT_MATRIX = new int[,]
        {
            {0, 0, 1},
            {0, 0, 0},
            {0, 0, 0}
        };

        private static int[,] TOP_LEFT_MATRIX = new int[,]
        {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 1}
        };

        //
        private static int[,] BOT_INT_RIGHT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {1, 0, 0},
            {2, 0, 0}
        };

        private static int[,] BOT_INT_LEFT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {0, 0, 1},
            {0, 0, 2}
        };

        private static int[,] TOP_INT_LEFT_MATRIX = new int[,]
        {
            {0, 0, 2},
            {0, 0, 1},
            {2, 1, 2}
        };

        private static int[,] TOP_INT_RIGHT_MATRIX = new int[,]
        {
            {2, 0, 0},
            {1, 0, 0},
            {2, 1, 2}
        };

        //
        private static int[,] BOT_INT_RIGHT_MATRIX2 = new int[,]
{
            {2, 2, 1},
            {1, 0, 0},
            {2, 0, 0}
};

        private static int[,] BOT_INT_LEFT_MATRIX2 = new int[,]
        {
            {2, 1, 2},
            {0, 0, 2},
            {0, 0, 1}
        };

        private static int[,] TOP_INT_LEFT_MATRIX2 = new int[,]
        {
            {0, 0, 2},
            {0, 0, 1},
            {1, 2, 2}
        };

        private static int[,] TOP_INT_RIGHT_MATRIX2 = new int[,]
        {
            {1, 0, 0},
            {2, 0, 0},
            {2, 1, 2}
        };

        //
        private static int[,] BOT_INT_RIGHT_MATRIX3 = new int[,]
        {
            {2, 1, 2},
            {2, 0, 0},
            {1, 0, 0}
        };

        private static int[,] BOT_INT_LEFT_MATRIX3 = new int[,]
        {
            {1, 2, 2},
            {0, 0, 1},
            {0, 0, 2}
        };

        private static int[,] TOP_INT_LEFT_MATRIX3 = new int[,]
        {
            {0, 0, 1},
            {0, 0, 2},
            {2, 1, 2}
        };

        private static int[,] TOP_INT_RIGHT_MATRIX3 = new int[,]
        {
            {2, 0, 0},
            {1, 0, 0},
            {2, 2, 1}
        };

        public static int NeedToFillAt(
            int[,] area,
            int i, int j,
            int margin)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = area[i + y + margin, j + x + margin];

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            bool needToFill = true;
            while (subAreaInt[1, 1] < maxValue && needToFill)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (subAreaInt[y, x] <= subAreaInt[1, 1])
                        {
                            subAreaBool[y, x] = false;
                        }
                        else
                        {
                            subAreaBool[y, x] = true;
                        }
                    }
                }

                needToFill = NeedToFill(ref subAreaBool);

                if (needToFill)
                {
                    subAreaInt[1, 1]++;
                }
            }

            return subAreaInt[1, 1];
        }

        public static int NeedToFillMaxAt(
            int[,] area,
            int i, int j,
            int margin)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = area[i + y + margin, j + x + margin];

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            bool needToFill = false;
            if (subAreaInt[1, 1] != maxValue)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (subAreaInt[y, x] != maxValue)
                        {
                            subAreaBool[y, x] = false;
                        }
                        else
                        {
                            subAreaBool[y, x] = true;
                        }
                    }
                }

                needToFill = NeedToFill(ref subAreaBool);
            }

            if (needToFill)
            {
                return maxValue;
            }
            return subAreaInt[1, 1];
        }

        public static int NeedToUnfillWaterAt(
            int[,] area,
            int i, int j,
            int margin)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = area[i + y + margin, j + x + margin];

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            return minValue;
        }

        public static int NeedToFillWaterAt(
            int[,] area,
            int i, int j,
            int margin)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = -1;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = area[i + y + margin, j + x + margin];

                    maxValue = Math.Max(maxValue, altitude);

                    if (altitude != -1)
                    {
                        if(minValue == -1)
                        {
                            minValue = altitude;
                        }
                        else
                        {
                            minValue = Math.Min(minValue, altitude);
                        }
                    }

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            //bool needToFill = false;
            if (maxValue >= 0 && subAreaInt[1, 1] != maxValue)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (subAreaInt[y, x] == -1)
                        {
                            subAreaBool[y, x] = false;
                        }
                        else
                        {
                            subAreaBool[y, x] = true;
                        }
                    }
                }

                if(NeedToFill(ref subAreaBool))
                {
                    return minValue;
                }
            }

            if (subAreaInt[1, 1] != -1)
            {
                return minValue;
            }
            return subAreaInt[1, 1];
            //if (needToFill)
            //{
            //    return minValue;
            //}
            //return subAreaInt[1, 1];
        }

        //public static int NeedToFillAltitudeLandAt(
        //    int[,] mountainArea,
        //    int i, int j)
        //{
        //    bool[,] subAreaBool = new bool[3, 3];
        //    int[,] subAreaInt = new int[3, 3];

        //    int maxValue = int.MinValue;
        //    int minValue = int.MaxValue;
        //    for (int y = -1; y < 2; y++)
        //    {
        //        for (int x = -1; x < 2; x++)
        //        {
        //            int altitude = mountainArea[i + y + 2, j + x + 2];

        //            maxValue = Math.Max(maxValue, altitude);

        //            minValue = Math.Min(minValue, altitude);

        //            subAreaInt[y + 1, x + 1] = altitude;
        //        }
        //    }

        //    bool needToFill = false;
        //    if (subAreaInt[1, 1] != maxValue)
        //    {
        //        for (int y = 0; y < 3; y++)
        //        {
        //            for (int x = 0; x < 3; x++)
        //            {
        //                if (subAreaInt[y, x] != maxValue)
        //                {
        //                    subAreaBool[y, x] = false;
        //                }
        //                else
        //                {
        //                    subAreaBool[y, x] = true;
        //                }
        //            }
        //        }

        //        needToFill = NeedToFill(ref subAreaBool);
        //    }

        //    if (needToFill)
        //    {
        //        return maxValue;
        //    }
        //    return subAreaInt[1, 1];
        //}

        private static bool MatchMatrix(ref bool[,] array, ref int[,] matrix)
        {
            bool result = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    switch (matrix[i, j])
                    {
                        case 0:
                            result &= array[i, j] == false;
                            break;
                        case 1:
                            result &= array[i, j];
                            break;
                    }
                    if(result == false)
                    {
                        return false;
                    }
                }
            }

            return result;
        }

        public static bool NeedToFill(ref bool[,] array)
        {
            if (MatchMatrix(ref array, ref HORIZONTAL_MATRIX)
                || MatchMatrix(ref array, ref VERTICAL_MATRIX)
                || MatchMatrix(ref array, ref CENTER_MATRIX)
                || MatchMatrix(ref array, ref BEND_BOT_LEFT_MATRIX)
                || MatchMatrix(ref array, ref BEND_BOT_RIGHT_MATRIX)
                || MatchMatrix(ref array, ref BEND_TOP_LEFT_MATRIX)
                || MatchMatrix(ref array, ref BEND_TOP_RIGHT_MATRIX))
            //|| MatchMatrix(ref array, ref CENTER_LEFT_MATRIX))
            {
                return true;
            }
            return false;
        }

        public static bool NeedToFillWater(ref bool[,] array)
        {
            if (MatchMatrix(ref array, ref BEND_WATER_MATRIX))
            //|| MatchMatrix(ref array, ref CENTER_LEFT_MATRIX))
            {
                return true;
            }
            return false;
        }

        public static LandTransition GetLandTransitionFrom(ref bool[,] array)
        {
            if (MatchMatrix(ref array, ref FULL_MATRIX))
            {
                return LandTransition.NONE;
            }

            //
            if (MatchMatrix(ref array, ref RIGHT_MATRIX))
            {
                return LandTransition.RIGHT;
            }
            if (MatchMatrix(ref array, ref BOT_MATRIX))
            {
                return LandTransition.BOT;
            }
            if (MatchMatrix(ref array, ref LEFT_MATRIX))
            {
                return LandTransition.LEFT;
            }
            if (MatchMatrix(ref array, ref TOP_MATRIX))
            {
                return LandTransition.TOP;
            }

            if (MatchMatrix(ref array, ref BOT_RIGHT_MATRIX))
            {
                return LandTransition.BOT_RIGHT;
            }
            if (MatchMatrix(ref array, ref BOT_LEFT_MATRIX))
            {
                return LandTransition.BOT_LEFT;
            }
            if (MatchMatrix(ref array, ref TOP_LEFT_MATRIX))
            {
                return LandTransition.TOP_LEFT;
            }
            if (MatchMatrix(ref array, ref TOP_RIGHT_MATRIX))
            {
                return LandTransition.TOP_RIGHT;
            }

            if (MatchMatrix(ref array, ref BOT_INT_RIGHT_MATRIX))
            {
                return LandTransition.BOT_INT_RIGHT;
            }
            if (MatchMatrix(ref array, ref BOT_INT_LEFT_MATRIX))
            {
                return LandTransition.BOT_INT_LEFT;
            }
            if (MatchMatrix(ref array, ref TOP_INT_LEFT_MATRIX))
            {
                return LandTransition.TOP_INT_LEFT;
            }
            if (MatchMatrix(ref array, ref TOP_INT_RIGHT_MATRIX))
            {
                return LandTransition.TOP_INT_RIGHT;
            }

            if (MatchMatrix(ref array, ref BOT_INT_RIGHT_MATRIX2)
                || MatchMatrix(ref array, ref BOT_INT_RIGHT_MATRIX3))
            {
                return LandTransition.BOT_INT_RIGHT;
            }
            if (MatchMatrix(ref array, ref BOT_INT_LEFT_MATRIX2)
                || MatchMatrix(ref array, ref BOT_INT_LEFT_MATRIX3))
            {
                return LandTransition.BOT_INT_LEFT;
            }
            if (MatchMatrix(ref array, ref TOP_INT_LEFT_MATRIX2)
                || MatchMatrix(ref array, ref TOP_INT_LEFT_MATRIX3))
            {
                return LandTransition.TOP_INT_LEFT;
            }
            if (MatchMatrix(ref array, ref TOP_INT_RIGHT_MATRIX2)
                || MatchMatrix(ref array, ref TOP_INT_RIGHT_MATRIX3))
            {
                return LandTransition.TOP_INT_RIGHT;
            }


            return LandTransition.NONE;
        }
    }
}
