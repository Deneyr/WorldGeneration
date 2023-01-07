using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks.ObjectLands.LandInterface;

namespace WorldGeneration.ObjectChunks.ObjectLands
{
    public static class LandTransitionHelper
    {
        private static int[,] FULL_MATRIX = new int[,]
        {
            {1, 1},
            {1, 1}
        };

        private static int[,] NONE_MATRIX = new int[,]
        {
            {0, 0},
            {0, 0}
        };

        private static int[,] RIGHT_MATRIX = new int[,]
        {
            {1, 0},
            {1, 0}
        };

        private static int[,] LEFT_MATRIX = new int[,]
        {
            {0, 1},
            {0, 1}
        };

        private static int[,] TOP_MATRIX = new int[,]
        {
            {0, 0},
            {1, 1}
        };

        private static int[,] BOT_MATRIX = new int[,]
        {
            {1, 1},
            {0, 0}
        };


        private static int[,] TOP_LEFT_MATRIX = new int[,]
        {
            {0, 0},
            {0, 1}
        };

        private static int[,] BOT_LEFT_MATRIX = new int[,]
        {
            {0, 1},
            {0, 0}
        };

        private static int[,] TOP_RIGHT_MATRIX = new int[,]
        {
            {0, 0},
            {1, 0}
        };

        private static int[,] BOT_RIGHT_MATRIX = new int[,]
        {
            {1, 0},
            {0, 0}
        };


        private static int[,] TOP_INT_LEFT_MATRIX = new int[,]
        {
            {0, 1},
            {1, 1}
        };

        private static int[,] BOT_INT_LEFT_MATRIX = new int[,]
        {
            {1, 1},
            {0, 1}
        };

        private static int[,] TOP_INT_RIGHT_MATRIX = new int[,]
        {
            {1, 0},
            {1, 1}
        };

        private static int[,] BOT_INT_RIGHT_MATRIX = new int[,]
        {
            {1, 1},
            {1, 0}
        };

        public static int[,] GetMatrixFromLandTransition(LandTransition landTransition)
        {
            switch (landTransition)
            {
                case LandTransition.NONE:
                    return FULL_MATRIX;
                case LandTransition.RIGHT:
                    return RIGHT_MATRIX;
                case LandTransition.LEFT:
                    return LEFT_MATRIX;
                case LandTransition.TOP:
                    return TOP_MATRIX;
                case LandTransition.BOT:
                    return BOT_MATRIX;
                case LandTransition.TOP_LEFT:
                    return TOP_LEFT_MATRIX;
                case LandTransition.BOT_LEFT:
                    return BOT_LEFT_MATRIX;
                case LandTransition.TOP_RIGHT:
                    return TOP_RIGHT_MATRIX;
                case LandTransition.BOT_RIGHT:
                    return BOT_RIGHT_MATRIX;
                case LandTransition.TOP_INT_LEFT:
                    return TOP_INT_LEFT_MATRIX;
                case LandTransition.BOT_INT_LEFT:
                    return BOT_INT_LEFT_MATRIX;
                case LandTransition.TOP_INT_RIGHT:
                    return TOP_INT_RIGHT_MATRIX;
                case LandTransition.BOT_INT_RIGHT:
                    return BOT_INT_RIGHT_MATRIX;
            }

            return NONE_MATRIX;
        }

        public static LandTransition GetLandTransitionFromMatrix(int[,] matrix)
        {
            if(matrix[0, 0] == 0 
                && matrix[0, 1] == 0
                && matrix[1, 0] == 0
                && matrix[1, 1] == 0)
            {
                return LandTransition.NONE;
            }

            if (matrix[0, 0] == 1
                && matrix[0, 1] == 0
                && matrix[1, 0] == 1
                && matrix[1, 1] == 0)
            {
                return LandTransition.RIGHT;
            }

            if (matrix[0, 0] == 0
                && matrix[0, 1] == 1
                && matrix[1, 0] == 0
                && matrix[1, 1] == 1)
            {
                return LandTransition.LEFT;
            }

            if (matrix[0, 0] == 0
                && matrix[0, 1] == 0
                && matrix[1, 0] == 1
                && matrix[1, 1] == 1)
            {
                return LandTransition.TOP;
            }

            if (matrix[0, 0] == 1
                && matrix[0, 1] == 1
                && matrix[1, 0] == 0
                && matrix[1, 1] == 0)
            {
                return LandTransition.BOT;
            }



            if (matrix[0, 0] == 0
                && matrix[0, 1] == 0
                && matrix[1, 0] == 0
                && matrix[1, 1] == 1)
            {
                return LandTransition.TOP_LEFT;
            }

            if (matrix[0, 0] == 0
                && matrix[0, 1] == 1
                && matrix[1, 0] == 0
                && matrix[1, 1] == 0)
            {
                return LandTransition.BOT_LEFT;
            }

            if (matrix[0, 0] == 0
                && matrix[0, 1] == 0
                && matrix[1, 0] == 1
                && matrix[1, 1] == 0)
            {
                return LandTransition.TOP_RIGHT;
            }

            if (matrix[0, 0] == 1
                && matrix[0, 1] == 0
                && matrix[1, 0] == 0
                && matrix[1, 1] == 0)
            {
                return LandTransition.BOT_RIGHT;
            }



            if (matrix[0, 0] == 0
                && matrix[0, 1] == 1
                && matrix[1, 0] == 1
                && matrix[1, 1] == 1)
            {
                return LandTransition.TOP_INT_LEFT;
            }

            if (matrix[0, 0] == 1
                && matrix[0, 1] == 1
                && matrix[1, 0] == 0
                && matrix[1, 1] == 1)
            {
                return LandTransition.BOT_INT_LEFT;
            }

            if (matrix[0, 0] == 1
                && matrix[0, 1] == 0
                && matrix[1, 0] == 1
                && matrix[1, 1] == 1)
            {
                return LandTransition.TOP_INT_RIGHT;
            }

            if (matrix[0, 0] == 1
                && matrix[0, 1] == 1
                && matrix[1, 0] == 1
                && matrix[1, 1] == 0)
            {
                return LandTransition.BOT_INT_RIGHT;
            }


            return LandTransition.NONE;
        }

        public static LandTransition ReverseLandTransition(LandTransition landTransition)
        {
            int[,] matrix = LandTransitionHelper.GetMatrixFromLandTransition(landTransition);

            int[,] reverseMatrix = new int[,]
            {
                { matrix[0, 0] == 1 ? 0 : 1, matrix[0, 1] == 1 ? 0 : 1 },
                { matrix[1, 0] == 1 ? 0 : 1, matrix[1, 1] == 1 ? 0 : 1 }
            };

            return LandTransitionHelper.GetLandTransitionFromMatrix(reverseMatrix);
        }

        public static LandTransition IntersectionLandTransition(LandTransition mainLandTransition, LandTransition secondLandTransition)
        {
            int[,] matrix1 = LandTransitionHelper.GetMatrixFromLandTransition(mainLandTransition);

            int[,] matrix2 = LandTransitionHelper.GetMatrixFromLandTransition(secondLandTransition);
            //if (secondLandTransition != LandTransition.NONE)
            //{
            //    matrix2 = LandTransitionHelper.GetMatrixFromLandTransition(secondLandTransition);
            //}
            //else
            //{
            //    matrix2 = FULL_MATRIX;
            //}

            int[,] intersectionMatrix = new int[,]
            {
                { (matrix1[0, 0] == 1 && matrix2[0, 0] == 1) ? 1 : 0, (matrix1[0, 1] == 1 && matrix2[0, 1] == 1) ? 1 : 0 },
                { (matrix1[1, 0] == 1 && matrix2[1, 0] == 1) ? 1 : 0, (matrix1[1, 1] == 1 && matrix2[1, 1] == 1) ? 1 : 0 }
            };

            return LandTransitionHelper.GetLandTransitionFromMatrix(intersectionMatrix);
        }

        public static LandTransition UnionLandTransition(LandTransition mainLandTransition, LandTransition secondLandTransition)
        {
            int[,] matrix1 = LandTransitionHelper.GetMatrixFromLandTransition(mainLandTransition);

            int[,] matrix2 = LandTransitionHelper.GetMatrixFromLandTransition(secondLandTransition);

            int[,] unionMatrix = new int[,]
            {
                { (matrix1[0, 0] == 1 || matrix2[0, 0] == 1) ? 1 : 0, (matrix1[0, 1] == 1 || matrix2[0, 1] == 1) ? 1 : 0 },
                { (matrix1[1, 0] == 1 || matrix2[1, 0] == 1) ? 1 : 0, (matrix1[1, 1] == 1 || matrix2[1, 1] == 1) ? 1 : 0 }
            };

            return LandTransitionHelper.GetLandTransitionFromMatrix(unionMatrix);
        }

        public static LandTransition CreateWallWaterTransition(ILandWall landWall, ILandWater landWater)
        {
            if (landWall != null
                && landWater == null)
            {
                return landWall.LandTransition;
            }

            if (landWater != null
                && landWall == null)
            {
                return ReverseLandTransition(landWater.LandTransition);
            }

            return IntersectionLandTransition(landWall.LandTransition, LandTransitionHelper.ReverseLandTransition(landWater.LandTransition));
        }

        public static List<ILandGround> AddFirstGroundLandObjectTo(List<ILandGround> landGrounds, ILandGround newGroundLandObject, LandTransition newGroundLandObjectTransition)
        {
            LandTransition landTransition = ReverseLandTransition(newGroundLandObjectTransition);

            List<ILandGround> resultLandGrounds = new List<ILandGround>();
            resultLandGrounds.Add(newGroundLandObject);

            if (newGroundLandObjectTransition != LandTransition.NONE)
            {
                foreach (ILandGround landGround in landGrounds)
                {
                    landGround.LandTransition = IntersectionLandTransition(landGround.LandTransition, landTransition);
                    if (landGround.LandTransition != LandTransition.NONE)
                    {
                        resultLandGrounds.Add(landGround);
                    }
                }

                resultLandGrounds = ClearHiddenGroundLandObjectsTo(resultLandGrounds);
            }

            return resultLandGrounds;
        }

        public static List<ILandGround> ClearHiddenGroundLandObjectsTo(List<ILandGround> landGrounds)
        {
            int[,] throughMatrix = new int[,]
            {
                {0, 0},
                {0, 0}
            };

            List<ILandGround> resultLandGrounds = new List<ILandGround>();

            int i = landGrounds.Count - 1;

            while(i >= 0 && IsFullMatrix(throughMatrix) == false)
            {
                ILandGround landGround = landGrounds[i];

                int[,] landGroundTransitionMatrix = GetMatrixFromLandTransition(landGround.LandTransition);

                resultLandGrounds.Insert(0, landGround);

                throughMatrix[0, 0] |= landGroundTransitionMatrix[0, 0];
                throughMatrix[0, 1] |= landGroundTransitionMatrix[0, 1];
                throughMatrix[1, 0] |= landGroundTransitionMatrix[1, 0];
                throughMatrix[1, 1] |= landGroundTransitionMatrix[1, 1];

                i--;
            }

            return resultLandGrounds;
        }

        public static bool IsFullMatrix(int[,] matrix)
        {
            return matrix[0, 0] == 1 && matrix[0, 1] == 1
                && matrix[1, 0] == 1 && matrix[1, 1] == 1;
        }
    }
}
