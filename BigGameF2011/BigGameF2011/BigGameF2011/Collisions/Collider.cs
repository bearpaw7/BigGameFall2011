using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BigGameF2011.GameObjects;

namespace BigGameF2011.Collisions
{
    /// <summary>
    /// This class is a combination of the Rectangle and Per-pixel collider code found at:
    /// http://create.msdn.com/en-US/education/catalog/tutorial/collision_2d_rectangle.
    /// Combining them should allow us high collision accuracy while minimizing speed loss.
    /// </summary>
    public class Collider
    {
        //Data Members
        private GameObject obj;
        private Texture2D Tex;
        private Rectangle firstCollider;
        private Color[] secondCollider;

        //Constructor
        public Collider(GameObject go, Texture2D gTexture)
        {
            obj = go;
            Tex = gTexture;
            Vector2 ObjPos = go.GetPosition();
            firstCollider = new Rectangle((int) (ObjPos.X-(Tex.Width/2)), (int) (ObjPos.Y-(Tex.Height/2)), Tex.Width, Tex.Height);
            secondCollider = new Color[(gTexture.Width * gTexture.Height)];
            gTexture.GetData(secondCollider);
        }

        public bool isCollidingWith(Collider other)
        {
            this.UpdatePosition();
            other.UpdatePosition();
            //if our rectangles don't intersect, then we aren't touching.
            if(!firstCollider.Intersects(other.firstCollider))
                return false;

            int top = Math.Max(this.firstCollider.Top, other.firstCollider.Top);
            int bottom = Math.Min(this.firstCollider.Bottom, other.firstCollider.Bottom);
            int left = Math.Max(this.firstCollider.Left, other.firstCollider.Left);
            int right = Math.Min(this.firstCollider.Right, other.firstCollider.Right);	
            Color self;
            Color them;

            //otherwise, check every pixel to find out if we are touching.
            for (int y = top; y < bottom; y++)
            {
	            for (int x = left; x < right; x++)
	            {
		            self = this.secondCollider[(x - this.firstCollider.Left) +
					            (y - this.firstCollider.Top) * this.firstCollider.Width];
		            them = other.secondCollider[(x - other.firstCollider.Left) +
					            (y - other.firstCollider.Top) * other.firstCollider.Width];

                    if (self.A != 0 && them.A != 0)
		            {
			            return true;
		            }
	            }
            }
            return false;
        }

        public void Triggered()
        {
            obj.OnCollision();
        }

        public void UpdatePosition()
        {
            Vector2 ObjPos = obj.GetPosition();
            firstCollider = new Rectangle((int)(ObjPos.X - (Tex.Width / 2)), (int)(ObjPos.Y - (Tex.Height / 2)), Tex.Width, Tex.Height);
//          firstCollider = new Rectangle((int)ObjPos.X,                     (int)ObjPos.Y,                      Tex.Width, Tex.Height);
        }

        public GameObject getGameObject()
        {
            return obj;
        }
    }
}
