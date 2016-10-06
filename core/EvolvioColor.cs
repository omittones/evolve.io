using System;
using core.Graphics;

namespace core
{
    public class EvolvioColor
    {
        public const double TIME_STEP = 0.001;
        
        public const int CREATURE_MINIMUM = 60;
        public const int ROCKS_TO_ADD = 0;
        public const int SEED = 48;
        public const string INITIAL_FILE_NAME = "DEFAULT";
        public const float MAX_TEMPERATURE = 1.0f;
        public const float MIN_TEMPERATURE = -0.5f;
        public const float NOISE_STEP_SIZE = 0.1f;
        public const float SCALE_TO_FIX_BUG = 100;
        public const int BOARD_HEIGHT = 100;
        public const int BOARD_WIDTH = 100;

        private int windowWidth;
        private int windowHeight;
        private float grossOverallScaleFactor;

        private Board evoBoard;
        private bool draggedFar;
        private float cameraR;
        private float cameraX = BOARD_WIDTH*0.5f;
        private float cameraY = BOARD_HEIGHT*0.5f;
        private float prevMouseX;
        private float prevMouseY;
        private float zoom = 1;
        private int dragging; // 0 = no drag, 1 = drag screen, 2 and 3 are dragging temp extremes.
        private PFont myFont;
        private GraphicsEngine graphics;

        public void setup(GraphicsEngine graphics, int width, int height)
        {
            this.windowWidth = width;
            this.windowHeight = height;
            this.grossOverallScaleFactor = ((float)windowHeight) / BOARD_HEIGHT / SCALE_TO_FIX_BUG;

            this.graphics = graphics;
            this.graphics.colorMode(ColorMode.HSB, 1.0f);
            myFont = this.graphics.loadFont("Jygquip1-48.vlw");
            evoBoard = new Board(this.graphics, BOARD_WIDTH, BOARD_HEIGHT, NOISE_STEP_SIZE,
                MIN_TEMPERATURE, MAX_TEMPERATURE,
                ROCKS_TO_ADD, CREATURE_MINIMUM, SEED,
                INITIAL_FILE_NAME, TIME_STEP);

            resetZoom();
        }

        public void draw()
        {
            for (var iteration = 0; iteration < evoBoard.playSpeed; iteration++)
            {
                evoBoard.iterate(TIME_STEP);
            }
            if (MathEx.Distance(prevMouseX, prevMouseY, this.graphics.mouseX, this.graphics.mouseY) > 5)
            {
                draggedFar = true;
            }
            if (dragging == 1)
            {
                cameraX -= toWorldXCoordinate(this.graphics.mouseX, this.graphics.mouseY) - toWorldXCoordinate(prevMouseX, prevMouseY);
                cameraY -= toWorldYCoordinate(this.graphics.mouseX, this.graphics.mouseY) - toWorldYCoordinate(prevMouseX, prevMouseY);
            }
            else if (dragging == 2)
            {
                //UGLY UGLY CODE.  Do not look at this
                if (evoBoard.setMinTemperature(1.0f - (this.graphics.mouseY - 30)/660.0f))
                {
                    dragging = 3;
                }
            }
            else if (dragging == 3)
            {
                if (evoBoard.setMaxTemperature(1.0f - (this.graphics.mouseY - 30)/660.0f))
                {
                    dragging = 2;
                }
            }
            if (evoBoard.userControl && evoBoard.selectedCreature != null)
            {
                cameraX = (float) evoBoard.selectedCreature.px;
                cameraY = (float) evoBoard.selectedCreature.py;
                cameraR = (float) (-Math.PI/2.0 - (float) evoBoard.selectedCreature.rotation);
            }
            else
            {
                cameraR = 0;
            }
            this.graphics.pushMatrix();
            this.graphics.scale(grossOverallScaleFactor);
            evoBoard.drawBlankBoard(SCALE_TO_FIX_BUG);
            this.graphics.translate(BOARD_WIDTH*0.5f*SCALE_TO_FIX_BUG, BOARD_HEIGHT*0.5f*SCALE_TO_FIX_BUG);
            this.graphics.scale(zoom);
            if (evoBoard.userControl && evoBoard.selectedCreature != null)
            {
                this.graphics.rotate(cameraR);
            }
            this.graphics.translate(-cameraX*SCALE_TO_FIX_BUG, -cameraY*SCALE_TO_FIX_BUG);
            evoBoard.drawBoard(SCALE_TO_FIX_BUG, zoom, (int) toWorldXCoordinate(this.graphics.mouseX, this.graphics.mouseY),
                (int) toWorldYCoordinate(this.graphics.mouseX, this.graphics.mouseY));
            this.graphics.popMatrix();
            evoBoard.drawUI(SCALE_TO_FIX_BUG, TIME_STEP, windowHeight, 0, windowWidth, windowHeight, myFont);

            evoBoard.fileSave();
            prevMouseX = this.graphics.mouseX;
            prevMouseY = this.graphics.mouseY;
        }

        public void mouseWheel(MouseEvent @event)
        {
            float delta = @event.getCount();
            if (delta >= 0.5)
            {
                setZoom(zoom*0.90909f, this.graphics.mouseX, this.graphics.mouseY);
            }
            else if (delta <= -0.5)
            {
                setZoom(zoom*1.1f, this.graphics.mouseX, this.graphics.mouseY);
            }
        }

        public void mousePressed()
        {
            if (this.graphics.mouseX < windowHeight)
            {
                dragging = 1;
            }
            else
            {
                if (Math.Abs(this.graphics.mouseX - (windowHeight + 65)) <= 60 && Math.Abs(this.graphics.mouseY - 147) <= 60 &&
                    evoBoard.selectedCreature != null)
                {
                    cameraX = (float) evoBoard.selectedCreature.px;
                    cameraY = (float) evoBoard.selectedCreature.py;
                    zoom = 4;
                }
                else if (this.graphics.mouseY >= 95 && this.graphics.mouseY < 135 && evoBoard.selectedCreature == null)
                {
                    if (this.graphics.mouseX >= windowHeight + 10 && this.graphics.mouseX < windowHeight + 230)
                    {
                        resetZoom();
                    }
                    else if (this.graphics.mouseX >= windowHeight + 240 && this.graphics.mouseX < windowHeight + 460)
                    {
                        evoBoard.creatureRankMetric = (evoBoard.creatureRankMetric + 1)%8;
                    }
                }
                else if (this.graphics.mouseY >= 570)
                {
                    float x = (this.graphics.mouseX - (windowHeight + 10));
                    float y = (this.graphics.mouseY - 570);
                    var clickedOnLeft = (x%230 < 110);
                    if (x >= 0 && x < 2*230 && y >= 0 && y < 4*50 && x%230 < 220 && y%50 < 40)
                    {
                        var mX = (int) (x/230);
                        var mY = (int) (y/50);
                        var buttonNum = mX + mY*2;
                        if (buttonNum == 0)
                        {
                            evoBoard.userControl = !evoBoard.userControl;
                        }
                        else if (buttonNum == 1)
                        {
                            if (clickedOnLeft)
                            {
                                evoBoard.creatureMinimum -= evoBoard.creatureMinimumIncrement;
                            }
                            else
                            {
                                evoBoard.creatureMinimum += evoBoard.creatureMinimumIncrement;
                            }
                        }
                        else if (buttonNum == 2)
                        {
                            evoBoard.prepareForFileSave(0);
                        }
                        else if (buttonNum == 3)
                        {
                            if (clickedOnLeft)
                            {
                                evoBoard.imageSaveInterval *= 0.5;
                            }
                            else
                            {
                                evoBoard.imageSaveInterval *= 2.0;
                            }
                            if (evoBoard.imageSaveInterval >= 0.7)
                            {
                                evoBoard.imageSaveInterval = Math.Round(evoBoard.imageSaveInterval);
                            }
                        }
                        else if (buttonNum == 4)
                        {
                            evoBoard.prepareForFileSave(2);
                        }
                        else if (buttonNum == 5)
                        {
                            if (clickedOnLeft)
                            {
                                evoBoard.textSaveInterval *= 0.5;
                            }
                            else
                            {
                                evoBoard.textSaveInterval *= 2.0;
                            }
                            if (evoBoard.textSaveInterval >= 0.7)
                            {
                                evoBoard.textSaveInterval = Math.Round(evoBoard.textSaveInterval);
                            }
                        }
                        else if (buttonNum == 6)
                        {
                            if (clickedOnLeft)
                            {
                                if (evoBoard.playSpeed >= 2)
                                {
                                    evoBoard.playSpeed /= 2;
                                }
                                else
                                {
                                    evoBoard.playSpeed = 0;
                                }
                            }
                            else
                            {
                                if (evoBoard.playSpeed == 0)
                                {
                                    evoBoard.playSpeed = 1;
                                }
                                else
                                {
                                    evoBoard.playSpeed *= 2;
                                }
                            }
                        }
                    }
                }
                else if (this.graphics.mouseX >= this.graphics.screenHeight + 10 && this.graphics.mouseX < this.graphics.screenWidth - 50 && evoBoard.selectedCreature == null)
                {
                    var listIndex = (this.graphics.mouseY - 150)/70;
                    if (listIndex >= 0 && listIndex < Board.LIST_SLOTS)
                    {
                        evoBoard.selectedCreature = evoBoard.list[listIndex];
                        cameraX = (float) evoBoard.selectedCreature.px;
                        cameraY = (float) evoBoard.selectedCreature.py;
                        zoom = 4;
                    }
                }
                if (this.graphics.mouseX >= this.graphics.screenWidth - 50)
                {
                    var toClickTemp = (this.graphics.mouseY - 30)/660.0f;
                    var lowTemp = 1.0f - evoBoard.getLowTempProportion();
                    var highTemp = 1.0f - evoBoard.getHighTempProportion();
                    if (Math.Abs(toClickTemp - lowTemp) < Math.Abs(toClickTemp - highTemp))
                    {
                        dragging = 2;
                    }
                    else
                    {
                        dragging = 3;
                    }
                }
            }
            draggedFar = false;
        }

        public void mouseReleased()
        {
            if (!draggedFar)
            {
                if (this.graphics.mouseX < windowHeight)
                {
                    // DO NOT LOOK AT THIS CODE EITHER it is bad
                    dragging = 1;
                    var mX = toWorldXCoordinate(this.graphics.mouseX, this.graphics.mouseY);
                    var mY = toWorldYCoordinate(this.graphics.mouseX, this.graphics.mouseY);
                    var x = (int) (Math.Floor(mX));
                    var y = (int) (Math.Floor(mY));
                    evoBoard.unselect();
                    cameraR = 0;
                    if (x >= 0 && x < BOARD_WIDTH && y >= 0 && y < BOARD_HEIGHT)
                    {
                        for (var i = 0; i < evoBoard.softBodiesInPositions[x, y].Count; i++)
                        {
                            var body = evoBoard.softBodiesInPositions[x, y][i];
                            if (body.isCreature)
                            {
                                var distance = MathEx.Distance(mX, mY, (float) body.px, (float) body.py);
                                if (distance <= body.getRadius())
                                {
                                    evoBoard.selectedCreature = (Creature) body;
                                }
                            }
                        }
                    }
                }
            }
            dragging = 0;
        }

        private void resetZoom()
        {
            cameraX = BOARD_WIDTH*0.5f;
            cameraY = BOARD_HEIGHT*0.5f;
            zoom = 1;
        }

        private void setZoom(float target, float x, float y)
        {
            var grossX = grossify(x, BOARD_WIDTH);
            cameraX -= (grossX/target - grossX/zoom);
            var grossY = grossify(y, BOARD_HEIGHT);
            cameraY -= (grossY/target - grossY/zoom);
            zoom = target;
        }

        private float grossify(float input, float total)
        {
            // Very weird function
            return (input/grossOverallScaleFactor - total*0.5f*SCALE_TO_FIX_BUG)/SCALE_TO_FIX_BUG;
        }

        private float toWorldXCoordinate(float x, float y)
        {
            var w = windowHeight/2.0f;
            var angle = (float) Math.Atan2(y - w, x - w);
            var dist2 = MathEx.Distance(w, w, x, y);
            return cameraX + grossify((float) Math.Cos(angle - cameraR)*dist2 + w, BOARD_WIDTH)/zoom;
        }

        private float toWorldYCoordinate(float x, float y)
        {
            var w = windowHeight/2.0f;
            var angle = (float) Math.Atan2(y - w, x - w);
            var dist2 = MathEx.Distance(w, w, x, y);
            return cameraY + grossify((float) Math.Sin(angle - cameraR)*dist2 + w, BOARD_HEIGHT)/zoom;
        }
    }
}