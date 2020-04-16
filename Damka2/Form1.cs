using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Damka2
{
    public partial class Form1 : Form
    {

        int turn = 1;
        myPictureBox checkersTool = null;
        bool isExtraMove = false;
        bool isSpecialMoveOfKing = false;
        int opponentThatIKill = 0;
        bool legalMove = false;



        List<myPictureBox> redToolList = new List<myPictureBox>();
        List<myPictureBox> greenToolList = new List<myPictureBox>();
        


        void createList()
        {
            
            redToolList.Add(redTool1);
            redToolList.Add(redTool2);
            redToolList.Add(redTool3);
            redToolList.Add(redTool4);          
            redToolList.Add(redTool5);       
            redToolList.Add(redTool6);
            redToolList.Add(redTool7);
            redToolList.Add(redTool8);
            redToolList.Add(redTool9);
            redToolList.Add(redTool10);
            redToolList.Add(redTool11);
            redToolList.Add(redTool12);
             
            greenToolList.Add(greenTool1);
            greenToolList.Add(greenTool2);
            greenToolList.Add(greenTool3);
            greenToolList.Add(greenTool4);
            greenToolList.Add(greenTool5);
            greenToolList.Add(greenTool6);
            greenToolList.Add(greenTool7);
            greenToolList.Add(greenTool8);
            greenToolList.Add(greenTool9);
            greenToolList.Add(greenTool10);
           greenToolList.Add(greenTool11);
           greenToolList.Add(greenTool12);

            
        }

        public Form1()
       {
           InitializeComponent();
           createList();
       }

       void highlightbackgroundColor(Object myObject)
       {
           if (!isExtraMove)
           {
               try { checkersTool.BackColor = Color.Silver; }
               catch { }
               myPictureBox mypictureBox = (myPictureBox)myObject;
               checkersTool = mypictureBox;
               checkersTool.BackColor = Color.Lime;

           }

       }


       void cellClick(object sender, MouseEventArgs e)
       {

           makeMove((myPictureBox)sender);

           if (legalMove && !isExtraMove)
           {
               if (isWinning())
               {
                   if (turn % 2 == 0)
                       MessageBox.Show("red is winning");
                   else
                       MessageBox.Show("green is winning");
               }

           }

           if (legalMove == true)
                  legalMove = false;



       }


       void makeMove(myPictureBox cell)
       {
           if (checkersTool != null)

           {
              bool isRealMove = true;
              string toolName = checkersTool.Name;

               if (isValidMove(checkersTool, cell, toolName, isRealMove)) ///valdition
               {

                   Point originPoint = new Point(checkersTool.myLocationX, checkersTool.myLocationY);
                   checkersTool.Location = cell.Location;
                   int advanceTool = checkersTool.myLocationY - cell.myLocationY;

                   checkersTool.myLocationX = cell.myLocationX;
                   checkersTool.myLocationY = cell.myLocationY;



                   if (!isSpecialMoveOfKing || isSpecialMoveOfKing && opponentThatIKill == 1)
                   {
                       if (!makeExtraMove(toolName, cell, originPoint) | Math.Abs(advanceTool) == 50) //verification movement extra
                       {
                           becomeToKing(toolName);
                           turn++; 
                           checkersTool.BackColor = Color.Silver;
                           checkersTool = null;
                           isExtraMove = false;
                       }
                       else
                       {
                           isExtraMove = true;

                       }
                   }
                   else
                   {
                       isSpecialMoveOfKing = false;
                       becomeToKing(toolName);
                       turn++;
                       checkersTool.BackColor = Color.Silver;
                       checkersTool = null;
                   }

                   legalMove = true;
                   opponentThatIKill = 0;


               }
           }
       }



       bool makeExtraMove(string toolName, myPictureBox cell,Point originPoint)
       {
           List<myPictureBox> opponentTools = null;
           List <Point> positions = new List<Point>();
           int sigPosition;


           if (toolName.Contains("red"))
           {
               opponentTools = greenToolList;
                   sigPosition = -100;
           }
           else
           {
               opponentTools = redToolList;
                   sigPosition = 100;
           }


           positions.Add(new Point(checkersTool.myLocationX + 100, checkersTool.myLocationY + sigPosition));
           positions.Add(new Point(checkersTool.myLocationX - 100, checkersTool.myLocationY + sigPosition));

           positions.Add(new Point(checkersTool.myLocationX + 100, checkersTool.myLocationY - sigPosition));
           positions.Add(new Point(checkersTool.myLocationX - 100, checkersTool.myLocationY - sigPosition));




           if (positions.Contains(originPoint))
           {
              positions.Remove(originPoint);
           }




           bool result = false;

           for(int i=0;i< positions.Count; i++)
           {
               if(positions[i].X >=50 && positions[i].X <=400 && positions[i].Y >= 50 && positions[i].Y <= 400)
               {

                   if (!isOccupiedCell(positions[i], redToolList) && !isOccupiedCell(positions[i], greenToolList))
                   {
                       Point average = new Point(averge(positions[i].X, checkersTool.myLocationX), averge(positions[i].Y, checkersTool.myLocationY));
                       if (isOccupiedCell(average, opponentTools))
                       {
                           result = true;
                       }
                   }
               }
           }

           return result;
       }


       int averge(int n1,int n2)
       {
           int result = n1 + n2;
           result = result / 2;
           return Math.Abs(result);


       }


       private bool isOccupiedCell(Point point, List <myPictureBox> list)
       {
           for(int i=0; i < list.Count; i++)
           {
               if(point.X == list[i].myLocationX && point.Y == list[i].myLocationY)
               {
                   return true;
               }

           }

           return false;

       }

       /// OVERLOADING OF isoccupiedCell

       bool isOccupiedCell(Point point, List<myPictureBox> side, ref int index)
       {
           for (int i = 0; i < side.Count; i++)
           {
               if (point.X == side[i].myLocationX && point.Y == side[i].myLocationY)
               {
                   index = i;
                   return true;
               }

           }
           return false;
       }


       bool isValidMove(myPictureBox currentBox, myPictureBox destinationBox,string toolName,bool isRealMove)
       {


           int advanceOfY = currentBox.myLocationY - destinationBox.myLocationY;
           int adnanceOfX = currentBox.myLocationX - destinationBox.myLocationX;

           if ((string)checkersTool.Tag != "king")
           {
               if (Math.Abs(advanceOfY) != Math.Abs(adnanceOfX))
                   return false;

           }          

           if (toolName.Contains("green"))
                advanceOfY = advanceOfY * -1;
           if ((string)checkersTool.Tag == "king")
                advanceOfY = Math.Abs(advanceOfY);

           if(isExtraMove && advanceOfY == -100)
                advanceOfY = Math.Abs(advanceOfY);


           if (advanceOfY == 50)
               return true;
           else if (advanceOfY == 100)
           {

               Point averagePoint = new Point(averge(currentBox.myLocationX, destinationBox.myLocationX), averge(currentBox.myLocationY, destinationBox.myLocationY));
               List<myPictureBox> opponentTools = null;
               if (toolName.Contains("red"))
                   opponentTools = greenToolList;
               else
                   opponentTools = redToolList;

               for(int i=0;i< opponentTools.Count;i++ )
               {
                   if(opponentTools[i].myLocationX == averagePoint.X && opponentTools[i].myLocationY == averagePoint.Y)
                   {
                       if (isRealMove)
                       {
                           opponentTools[i].Location = new Point(0, 0);
                           opponentTools[i].myLocationX = 0;
                           opponentTools[i].myLocationY = 0;
                           opponentTools[i].Visible = false;
                           opponentTools.RemoveAt(i);

                       }



                       return true;
                   }
               }
           }


           if ((string)checkersTool.Tag == "king")
           {
               if(isDiagonal(currentBox, destinationBox))
               {
                   List<myPictureBox> opponentTools = null;
                   List<myPictureBox> myTools = null;


                   if (toolName.Contains("red"))
                   {
                       opponentTools = greenToolList;
                       myTools = redToolList;
                   }
                   else
                   {
                       opponentTools = redToolList;
                       myTools = greenToolList;
                   }



                   int tempX = destinationBox.myLocationX - currentBox.myLocationX;
                   int tempY = destinationBox.myLocationY - currentBox.myLocationY;

                   tempX = tempX > 0 ? 50 : -50;
                   tempY = tempY > 0 ? 50 : -50;

                   int xCoordinate = tempX + currentBox.myLocationX;
                   int yCoordinate = currentBox.myLocationY + tempY;

                   Point pointToCheck = new Point(xCoordinate, yCoordinate);
                   Point pointToRemove = new Point(0,0);
                   int indexToremove=-1;


                   int biggerRangeOfX = xCoordinate > destinationBox.myLocationX ? xCoordinate : destinationBox.myLocationX;
                   int smallerRangeOfX = xCoordinate < destinationBox.myLocationX ? xCoordinate : destinationBox.myLocationX;


                   int biggerRangeOfY = yCoordinate > destinationBox.myLocationY ? yCoordinate : destinationBox.myLocationY;
                   int smallerRangeOfY = yCoordinate < destinationBox.myLocationY ? yCoordinate : destinationBox.myLocationY;





                   for (; ( ((smallerRangeOfX <= xCoordinate) && (xCoordinate <= biggerRangeOfX)) && ((smallerRangeOfY <= yCoordinate) && (yCoordinate <= biggerRangeOfY)) ) ; xCoordinate+= tempX, yCoordinate+= tempY)
                   {

                       pointToCheck.X = xCoordinate;
                       pointToCheck.Y = yCoordinate;


                      if (isOccupiedCell(pointToCheck, opponentTools, ref indexToremove))
                      {
                           if (isRealMove)
                           {
                               opponentThatIKill++;
                               if (opponentThatIKill == 2)
                               {
                                   opponentThatIKill = 0;
                                   return false;
                               }

                           }                              
                           continue;
                      }
                      if(isOccupiedCell(pointToCheck, myTools))
                      {
                        return false;
                      }

                   }

                   if(indexToremove != -1)
                   {
                       if (isRealMove)
                       {
                           opponentTools[indexToremove].Location = new Point(0, 0);
                           opponentTools[indexToremove].myLocationX = 0;
                           opponentTools[indexToremove].myLocationY = 0;
                           opponentTools[indexToremove].Visible = false;
                           opponentTools.RemoveAt(indexToremove);
                       }
                   }
                   if (isRealMove)
                   {
                       isSpecialMoveOfKing = true;
                   }

                   return true;
               }                    
           }
           return false;

       }




       bool isDiagonal(myPictureBox currentBox, myPictureBox destinationBox)
       {

           int diffLine = Math.Abs(currentBox.myLocationX - destinationBox.myLocationX);
           int diffColumn = Math.Abs(currentBox.myLocationY - destinationBox.myLocationY);

           if (diffLine == diffColumn)
               return true;

           return false;
       }




       bool isWinning()
       {

           List<myPictureBox> ToolsToCheckWin = null;

           if (turn % 2 == 0)
               ToolsToCheckWin = greenToolList;
           else
               ToolsToCheckWin = redToolList;


           if ((ToolsToCheckWin.Count == 0))
               return true;



           myPictureBox cellForSimulate = new myPictureBox();
           cellForSimulate.myLocationX = 50;
           cellForSimulate.myLocationY = 50;
           Point pointToCheck = new Point(50, 50);
           bool isRealMove = false;


           for (int i = 0; i < ToolsToCheckWin.Count; i++)
           {

               int evenRow = 0;

               for (int j=50; j<= 400; j +=50)
               {

                   if(evenRow % 2 == 0)
                       for (int k=50;k<=350;k += 100)
                       {
                           cellForSimulate.myLocationX = k;
                           cellForSimulate.myLocationY = j;
                           pointToCheck.X = k;
                           pointToCheck.Y = j;
                           string tooName = ToolsToCheckWin[i].Name;
                           checkersTool = ToolsToCheckWin[i];

                           if (isValidMove(checkersTool, cellForSimulate, tooName, isRealMove) && (!isOccupiedCell(pointToCheck, greenToolList) && !isOccupiedCell(pointToCheck, redToolList)))
                               return false;

                       }
                   else
                   {
                       for(int k = 100; k <= 400; k += 100)
                       {
                           cellForSimulate.myLocationX = k;
                           cellForSimulate.myLocationY = j;
                           pointToCheck.X = k;
                           pointToCheck.Y = j;
                           string tooName = ToolsToCheckWin[i].Name;
                           if (isValidMove(checkersTool, cellForSimulate, tooName, isRealMove) && (!isOccupiedCell(pointToCheck, greenToolList) && !isOccupiedCell(pointToCheck, redToolList)))
                               return false;

                       }
                   }

                   evenRow++;
               }

           }


           return true;
       }



          void becomeToKing(string toolName)
        {

            if (toolName.Contains("green") && checkersTool.myLocationY == 400)
            {
                checkersTool.BackgroundImage = Damka2.Properties.Resources.king_green;
                checkersTool.Tag = "king";
            }


            if (toolName.Contains("red") && checkersTool.myLocationY == 50)
            {
                checkersTool.BackgroundImage = Damka2.Properties.Resources.king_red;
                checkersTool.Tag = "king";
            }


        }


        void selectGreenTool(object sender, MouseEventArgs e)
        {
            if (turn % 2 == 0)
            {
                highlightbackgroundColor(sender);
            }
            else
                MessageBox.Show("this is red turn");
            
        }

        void selectRedTool(object sender, MouseEventArgs e)
        {
            if (turn % 2 == 1)
            {
                highlightbackgroundColor(sender);
            }
            else
                MessageBox.Show("this is green turn");
        }

       
    }
}
