using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public class Planet {
		public int distance;
		public string name;
		public Color skyParticleColor;
		public Color cloudColor;
		public Color groundColor;
		public Color mountainColor;
		public Color backgroundColor;
		public Color vignetteColor;
		public Material skyParticles;
		public int gravity;
		public int radius;

	};

	private Planet[] planets = new Planet[8];
	private int distanceToNextPlanet = 0;
	public float playerHeight = 0.0f;
	public SpriteRenderer vignette;
	public SpriteRenderer backGroundColor;
	public ParticleSystem skyParticle;
	public ParticleSystem clouds;
	public ParticleSystemRenderer sP;
	public ParticleSystemRenderer cL;
	public Material mountainMaterial;
	public SpriteRenderer planetBackground;
	public Player player;
	public float currentPlanetRadius;

	// Use this for initialization
	void Start () {
		sP = (ParticleSystemRenderer)skyParticle.GetComponent<Renderer>();
		cL = (ParticleSystemRenderer)clouds.GetComponent<Renderer>();
		planets[0] = new Planet();  
		planets[0].distance = 0;
		planets[0].name = "Earth";
		planets[0].radius = 300;
		planets[0].gravity = -4;
		planets[0].skyParticles = (Material)Resources.Load("Materials/SkyParticle");
		planets[0].cloudColor = new Color(1,1,1);
		planets[0].skyParticleColor = new Color(1, 1, 1);
		planets[0].mountainColor = new Color(1, 0.55f, 0);
		planets[0].groundColor = new Color(0, 0.22f, 0.35f);
		planets[0].vignetteColor = new Color(0, 0.42f, 0.63f);
		planets[0].backgroundColor = new Color(0, 0.32f, 0.53f);

		planets[1] = new Planet();
		planets[1].distance = 1500;
		planets[1].name = "Mars";
		planets[1].radius = 500;
		planets[1].gravity = -2;
		planets[1].skyParticles = (Material)Resources.Load("Materials/SkyParticle2");
		planets[1].cloudColor = new Color(0.74f,0.32f,0, 0.7f);
		planets[1].skyParticleColor = new Color(1, 0, 0);
		planets[1].mountainColor = new Color(1, 0.18f, 0);
		planets[1].groundColor = new Color(0, 0.35f, 0.21f);
		planets[1].vignetteColor = new Color(0, 0.16f, 0.13f);
		planets[1].backgroundColor = new Color(1, 0.93f, 0.69f);

	

		UpdatePlanet(1);

	}
	
	// Update is called once per frame
	void Update () {
		playerHeight = player.transform.position.y;
		Color temp = backGroundColor.color;
		if(playerHeight > currentPlanetRadius) {
			backGroundColor.color = new Color(temp.r, temp.g, temp.b, 1 - ((playerHeight-100)*0.05f));
		}
	}

	void UpdatePlanet(int a) {
		Physics.gravity = new Vector3(0, planets[a].gravity, 0);
		backGroundColor.color = planets[a].backgroundColor;
		vignette.color = planets[a].vignetteColor;
		sP.material = planets[a].skyParticles;
		mountainMaterial.color = planets[a].mountainColor;
		cL.material.color = planets[a].cloudColor;
		clouds.startColor = planets[a].cloudColor;
		planetBackground.color = planets[a].groundColor;
	}
}
