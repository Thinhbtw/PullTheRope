using System.Collections;
using UnityEngine;

public class LinesDrawer : Singleton<LinesDrawer> {

	[SerializeField] GameObject textTutorial;
	[SerializeField] Transform levelHolder;
	[SerializeField] LineRenderer warningLine;
	public CountDown text;
	public float MaxLenghtLine; //dong` 131
	[Space(30f)]

	public GameObject linePrefab;
	public LayerMask cantDrawOverLayer;
	int cantDrawOverLayerIndex;

	[Space ( 30f )]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;

	public Rope myRope;

	public Transform _target;
	public bool cantDraw;
	bool canCountinueToDraw;
	Line currentLine;
	RaycastHit2D t;

	Camera cam;

	[SerializeField] GameObject uiComplete;

	void Start ( ) {
		cam = Camera.main;
		cantDrawOverLayerIndex = LayerMask.NameToLayer ( "CantDrawOver" );
		cantDraw = false;
		Input.multiTouchEnabled = false;
		canCountinueToDraw = true;
	}

	void Update() {

		if (Input.GetMouseButtonDown(0))
			BeginDraw();

		if (currentLine != null)
		{
			Draw();
		}

		if (Input.GetMouseButtonUp(0))
        {
			EndDraw ( );
        }
	}

	bool SwitchNameCollsion(string name)
	{
		switch (name)
		{
			case var s when name.Contains("TrueObj"): return false;
			case var s when name.Contains("trash"): return false;
			case var s when name.Contains("Coc"): return false;
		}
		return true;
	}

	// Begin Draw ----------------------------------------------
	void BeginDraw ( ) { 
		currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );

		//Set line properties
		currentLine.UsePhysics ( false );
		currentLine.SetLineColor ( lineColor );
		currentLine.SetPointsMinDistance ( linePointsMinDistance );
		currentLine.SetLineWidth ( lineWidth );

	}
	// Draw ----------------------------------------------------
	
	void Draw ( ) {
		if (cantDraw)
			return;
		Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
		//Check if mousePos hits any collider with layer "CantDrawOver"
		RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
		if (currentLine.GetLineRendererCount() > 0)
		{
			t = Physics2D.Linecast(currentLine.GetLastPoint(), mousePosition, cantDrawOverLayer);
			canCountinueToDraw = (t.collider == null) ? true : SwitchNameCollsion(t.collider.name);
			if (!canCountinueToDraw) //them getline tai. vi` loi~ co' loi~ neu ko ve dc duong` den nhung ve dc duogn` do?
			{
				warningLine.positionCount = 2;
				warningLine.SetPosition(0, currentLine.GetLastPoint());
				warningLine.SetPosition(1, mousePosition);
			}
			else
            {
				warningLine.positionCount = 0;
			}
		}
		if (!hit && canCountinueToDraw)
		{
			currentLine.AddPoint(mousePosition);
		}
		/*if (hit)
		{
			canCountinueToDraw = false;
			return;
		}*/
		
	}
	// End Draw ------------------------------------------------
	void EndDraw ( ) {
        if (currentLine != null)
        {
            if (currentLine.pointsCount < 2)
            {
                //If line has one point
                Destroy(currentLine.gameObject);
            }
            else
            {
				//Add the line to "CantDrawOver" layer
				//currentLine.gameObject.layer = cantDrawOverLayerIndex;

				currentLine.lineRenderer.SetPosition(0, _target.position);
				currentLine.AddPoint(_target.position);

                if (currentLine.LengthLine() > MaxLenghtLine)
                {
                    Destroy(currentLine.gameObject);
                    currentLine = null;
                    canCountinueToDraw = true;
                    return;
                }

                cantDraw = canCountinueToDraw = true;

				//tat' text tutorial
				textTutorial.SetActive(false);

				//hint
				levelHolder.GetChild(0).GetChild(1).gameObject.SetActive(false);

				if(!uiComplete.activeInHierarchy) //level 37 39 40 neu' bat. complete ma` chua tha? net ve thi van co tieng
                {
					SoundManager.Instance.PlaySound(SoundManager.SoundType.PullRope);
                }
				myRope.gameObject.SetActive(true);
				text.hasntCountDown = false;

            }
			warningLine.positionCount = 0;
		} 
        
	}
	public void ResetLevel()
    {
		cantDraw = false;
		myRope.gameObject.SetActive(false);
		canCountinueToDraw = true;
		if (this.transform.childCount > 0)
			Destroy(this.transform.GetChild(0).gameObject);
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			if(currentLine != null)
            {
				if (currentLine.pointsCount > 3)
				{
					textTutorial.SetActive(false);
				}
				EndDraw();
            }
		}
	}
}
