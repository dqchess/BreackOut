using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
// Generate Stage Background Texture!
/// </summary>
public class StageTexture {
	//private variables
	private Texture2D _sourceTex;
	private Texture2D _stageTexture = null;
	public int cornorX = 10;
	public int cornorY = 10;

	//init function
	public StageTexture(int sizeX,int sizeY,string file){
		_sourceTex = (Texture2D)Resources.Load(file,typeof(Texture2D));
		cornorX = sizeX;
		cornorY = sizeY;
	}

	//texture to bytes; 
	public byte[] bytes(){
		if(_stageTexture){
			return _stageTexture.EncodeToPNG();
		}
		return null;
	}

	//get texture 
	public Texture2D tex(){
		return _stageTexture;
	}

	//remove texture
	public void reset(){
		_stageTexture = null;
	}

	//generate texture
	public void stageTextureUpdate(Vector2 tileSize,Vector2 tileNum,int[,] _boards){
		int _sizeX = (int)tileSize.x;
		int _sizeY = (int)tileSize.y;
		int _numX = (int)tileNum.x;
		int _numY = (int)tileNum.y;
		int totalX = _sizeX*_numX + cornorX*(_numX+1);
		int totalY = _sizeY*_numY + cornorY*(_numY+1);
		
		reset();
		Color[] transparency = new Color[totalX*totalY];
		Color c = new Color(70,70,70,0);
		for(int i =0;i<totalX*totalY;i++){
			transparency[i] = c;
		}

		_stageTexture = new Texture2D(totalX,totalY,TextureFormat.RGBA32, false);
		_stageTexture.SetPixels(0,0,totalX,totalY,transparency);

		bool toggler = true;
		for(int x=0;x <= _numX ; x++){
			for(int y=0;y <= _numY ; y++){
				//toggler = !toggler;

				int sx = _sizeX;
				int sy = _sizeY;
				int _x = x*sx+cornorX*(x+1);
				int _y = totalY - (y*sy+cornorY*(y+1)) - sy ;

				bool b1 = (x==0||y==0||_boards[y-1,x-1]==-2);
				bool b2 = (x==0||y>=_numY||_boards[y,x-1]==-2);
				bool b3 = (y==0||x>=_numX||_boards[y-1,x]==-2);
				bool b4 = (y>=_numY||x>=_numX||_boards[y,x]==-2);

				if( b1 && b2 && b3 && !b4 )
					paintC1(_stageTexture,_x,_y,sx,sy);
				if( b1 && !b2 && b3 && b4 )
					paintC2(_stageTexture,_x,_y,sx,sy);
				if( b1 && b2 && !b3 && b4 )
					paintC3(_stageTexture,_x,_y,sx,sy);
				if( !b1 && b2 && b3 && b4 )
					paintC4(_stageTexture,_x,_y,sx,sy);

				if( !b1 && !b2 && !b3 && b4 )
					paintC5(_stageTexture,_x,_y,sx,sy);
				if( !b1 && b2 && !b3 && !b4 )
					paintC6(_stageTexture,_x,_y,sx,sy);
				if( !b1 && !b2 && b3 && !b4 )
					paintC7(_stageTexture,_x,_y,sx,sy);
				if( b1 && !b2 && !b3 && !b4 )
					paintC8(_stageTexture,_x,_y,sx,sy);

				if( b1 && b2 && !b3 && !b4)
					paintSpaceL(_stageTexture,_x,_y,sx,sy);
				if( !b1 && !b2 && b3 && b4)
					paintSpaceR(_stageTexture,_x,_y,sx,sy);
				if( b1 && !b2 && b3 && !b4)
					paintSpaceT(_stageTexture,_x,_y,sx,sy);
				if( !b1 && b2 && !b3 && b4)
					paintSpaceB(_stageTexture,_x,_y,sx,sy);
				
				if( b3 && !b4 )
					paintMT(_stageTexture,_x,_y,sx,sy);
				if( !b3 && b4 )
					paintMB(_stageTexture,_x,_y,sx,sy);
				
				if( !b2 && b4 )
					paintRM(_stageTexture,_x,_y,sx,sy);
				if( b2 && !b4 )
					paintLM(_stageTexture,_x,_y,sx,sy);
				
				if( b1 && !b2 && !b3 && b4)
					paintCross(_stageTexture,_x,_y,sx,sy);
				if( !b1 && b2 && b3 && !b4)
					paintCross(_stageTexture,_x,_y,sx,sy);

				if( !b2 && !b4)
					paintSL(_stageTexture,_x,_y,sx,sy,toggler);
				if( !b3 && !b4)
					paintST(_stageTexture,_x,_y,sx,sy,toggler);
				if( !b1 && !b2 && !b3 && !b4)
					paintSLT(_stageTexture,_x,_y,sx,sy,toggler);
				if( !b4 )
					paintMM(_stageTexture,_x,_y,sx,sy,toggler);
			}
		}
		_stageTexture.Apply();
	}

	//Get pixel from origin texture
	private Color[] getSourcePixel(int x,int y){
		return _sourceTex.GetPixels(cornorX*x, cornorY*y, cornorX, cornorY);
	}
	
	//Array Slice-Copy
	T[] ArrayFromRange<T>(T[] originalArray, int startIndex, int length)
	{
		int actualLength = Math.Min(length, originalArray.Length - startIndex);
		T[] copy = new T[actualLength];
		Array.Copy(originalArray, startIndex, copy, 0, actualLength);
		return copy;
	}
	
	//expand texture pixel.
	Color[] RectCopy(Color[] source,int sizeX,int sizeY,int sx,int sy){
		Color[] cs1 = HorizontalCopy(source,sizeX,sizeY,sx);
		Color[] cs2 = VerticalCopy(cs1,sx,sizeY,sy);
		return cs2;
	}

	//expand texture (Horizon Copy)
	Color[] HorizontalCopy(Color[] source,int sizeX,int sizeY,int sx){
		Color[] dist = new Color[sx*sizeY];
		int count = sx/sizeX;
		int remain = sx%sizeX;
		for(int j=0;j<sizeY;j++){
			Color[] line = new Color[sx];
			Color[] _ = ArrayFromRange(source,j*sizeX,sizeX);
			for(int i=0;i<count;i++)
				_.CopyTo(line,i*sizeX);
			if(remain > 0)
				ArrayFromRange(source,j*sizeX,remain).CopyTo(line,sizeX*count);
			line.CopyTo(dist,j*sx);
		}
		return dist;
	}

	//expand texture (Vertical Copy)
	Color[] VerticalCopy(Color[] source,int sizeX,int sizeY,int sy){
		Color[] dist = new Color[sy*sizeX];
		int count = sy/sizeY;
		int remain = sy%sizeY;
		for(int i=0;i<count;i++)
			source.CopyTo(dist,i*(sizeY*sizeX));
		if(remain > 0)
			ArrayFromRange(source,0,sizeX*remain).CopyTo(dist,(sizeY*sizeX)*count);
		return dist;
	}

	//Get horizon outline 
	Color[] HorizontalCorner(int tileX,int tileY,int sx){
		Color[] s = getSourcePixel(tileX,tileY);
		return HorizontalCopy(s,cornorX,cornorY,sx);
	}

	//Get vertical outline
	Color[] VerticalCorner(int tileX,int tileY,int sy){
		Color[] s = getSourcePixel(tileX,tileY);
		return VerticalCopy(s,cornorX,cornorY,sy);
	}

	///////////
	//C1-C2-C3
	//C4-XX-C5
	//C6-C7-C8
	///////////
	// get C1 Outline
	private void paintC1(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(0,3);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}
	// get C2 Outline
	private void paintC2(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(2,3);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}
	// get C3 Outline
	private void paintC3(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(0,1);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}
	// get C4 Outline
	private void paintC4(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(2,1);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}
	// get C5 Outline
	private void paintC5(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(3,3);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}
	// get C6 Outline
	private void paintC6(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(3,2);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}
	// get C7 Outline
	private void paintC7(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(3,1);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}
	// get C8 Outline
	private void paintC8(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(3,0);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);
	}

	//////////
	//-- T --
	//L- X -R
	//-- B --
	//////////
	// Get top margin pixels
	private void paintSpaceT(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(1,3);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);	
	}
	// Get left margin pixels
	private void paintSpaceL(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(0,2);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);	
	}
	// Get right margin pixels
	private void paintSpaceR(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(2,2);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);	
	}
	// Get bottom margin pixels
	private void paintSpaceB(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(1,1);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);	
	}

	//////////
	//   MT
	//LM-MM-RM
	//   MB
	//////////
	// get Middle-Top outline 
	private void paintMT(Texture2D texture,int x,int y,int sx,int sy){
		Color[] tHorizon = HorizontalCorner(1,3,sx);
		_stageTexture.SetPixels(x,y+sy,sx,cornorY,tHorizon);
	}
	// get Left-Middle outline 
	private void paintLM(Texture2D texture,int x,int y,int sx,int sy){
		Color[] lVertical = VerticalCorner(0,2,sy);
		_stageTexture.SetPixels(x-cornorX,y,cornorX,sy,lVertical);
	}
	// get Middle-Bottom outline 
	private void paintMB(Texture2D texture,int x,int y,int sx,int sy){
		Color[] tHorizon = HorizontalCorner(1,1,sx);
		_stageTexture.SetPixels(x,y+sy,sx,cornorY,tHorizon);
	}
	// get Right-Bottom outline 
	private void paintRM(Texture2D texture,int x,int y,int sx,int sy){
		Color[] lVertical = VerticalCorner(2,2,sy);
		_stageTexture.SetPixels(x-cornorX,y,cornorX,sy,lVertical);
	}
	// get Middle-Middle outline 
	private void paintMM(Texture2D texture,int x,int y,int sx,int sy,bool toggle){
		Color[] s = null;
		if(toggle){
			s = getSourcePixel(0,0);
		}else{
			s = getSourcePixel(1,0);
		}
		s = RectCopy(s,cornorX,cornorY,sx,sy);
		_stageTexture.SetPixels(x,y,sx,sy,s);
	}
	// get texture gap left-right 
	private void paintSL(Texture2D texture,int x,int y,int sx,int sy,bool toggle){
		Color[] s = null;
		if(toggle){
			s = VerticalCorner(0,0,sy);
		}else{
			s = VerticalCorner(1,0,sy);
		}
		
		_stageTexture.SetPixels(x-cornorX,y,cornorX,sy,s);
	}
	// get texture gap top-bottom 
	private void paintST(Texture2D texture,int x,int y,int sx,int sy,bool toggle){
		Color[] s = null;
		if(toggle){
			s = HorizontalCorner(0,0,sy);
		}else{
			s = HorizontalCorner(1,0,sy);
		}
		_stageTexture.SetPixels(x,y+sy,sx,cornorY,s);
	}
	// get texture gap top-bottom-left-right
	private void paintSLT(Texture2D texture,int x,int y,int sx,int sy,bool toggle){
		Color[] s = null;
		if(toggle){
			s = getSourcePixel(0,0);
		}else{
			s = getSourcePixel(1,0);
		}
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,s);
	}
	// get cross cornor texture  
	private void paintCross(Texture2D texture,int x,int y,int sx,int sy){
		Color[] cs = getSourcePixel(2,0);
		_stageTexture.SetPixels(x-cornorX,y+sy,cornorX,cornorY,cs);	
	}
}
