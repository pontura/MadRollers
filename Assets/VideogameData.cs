using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AlpacaSound.RetroPixelPro;

[Serializable]
public class VideogameData {

	public int id;
	public string name;
	public Color floor_top;
	public Color floor_border;
	public Material wallMaterial;
    public Color bossFog;
    public Color fog;
	public Sprite logo;
	public Sprite floppyCover;
	public Sprite loadingSplash;
	public Sprite intro_logo;
	public RetroPixelPro retroPixelPro;
	public string credits;

}
