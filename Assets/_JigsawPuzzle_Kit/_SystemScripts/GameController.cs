//-----------------------------------------------------------------------------------------------------	
// Script controls whole gameplay, UI and all sounds
//-----------------------------------------------------------------------------------------------------	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


[AddComponentMenu("Scripts/Jigsaw Puzzle/Game Controller")]
public class GameController : MonoBehaviour 
{

	public Camera gameCamera;
	public PuzzleController puzzle;

	// Background (assembled puzzle preview)
	public Renderer background;
	public bool adjustBackground = true;
	public float backgroundTransparency = 0.1f;
	
	public AudioClip musicMain;
	public AudioClip musicWin;

	public AudioClip soundGrab;
	public AudioClip soundDrop;
	public AudioClip soundAssemble;
	public bool invertRules = false;	// Allows to invert basic rules - i.e. player should decompose  the images



	// Important internal variables - please don't change them blindly
	CameraController cameraScript;
	bool gameFinished;
	Color backgroundColor;
	static Vector3 oldPointerPosition;



    //=====================================================================================================
    // Initialize
    void OnEnable () 
	{ 
		// Prepare Camera
		if (!gameCamera) 
			gameCamera = Camera.main;
		
		gameCamera.orthographic = true;
		cameraScript = gameCamera.GetComponent<CameraController>();

		// Initiate puzzle and prepare background
		if (StartPuzzle (puzzle))
		{
			puzzle.SetPiecesActive(true); 
			PrepareBackground (background);
		}


		// Load saved data
		Load ();

		Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.Confined;

        if (!puzzle)
			this.enabled = false;
	}

	//-----------------------------------------------------------------------------------------------------	
	// Main game cycle
	void Update () 
	{
		if (puzzle  &&  Time.timeScale > 0  &&  !gameFinished)
		{
			// Process puzzle and react on it state
            switch (puzzle.ProcessPuzzle (
                                            GetPointerPosition(gameCamera), 
                                            !EventSystem.current.IsPointerOverGameObject()  &&  Input.GetMouseButton(0)  &&  (!cameraScript || !cameraScript.IsCameraMoved()),
                                            GetRotationDirection()
                                          ) )
			{
				case PuzzleState.None:
					;
					break;

				case PuzzleState.DragPiece:
					AudioManager.getInstance().PlayAudio(soundGrab);
					break;

				case PuzzleState.ReturnPiece:
					AudioManager.getInstance().PlayAudio(soundAssemble);
					break;

				case PuzzleState.DropPiece:
					AudioManager.getInstance().PlayAudio(soundDrop);
					break;

				// Hide all pieces and finish game - if whole puzzle Assembled 	
				case PuzzleState.PuzzleAssembled:
					if (background && !invertRules) 
						puzzle.SetPiecesActive(true);

					AudioManager.getInstance().PlayAudio(musicWin);
					gameFinished = true;
					break;	
			}
            
		}


		// Control Camera   
        if (cameraScript && puzzle)
            cameraScript.enabled = (puzzle.GetCurrentPiece() == null);
	}

	//-----------------------------------------------------------------------------------------------------	 
	// Get current pointer(mouse or single touch) position  
	static Vector3 GetPointerPosition (Camera _camera) 
	{
		Vector3 pointerPosition = oldPointerPosition;

		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
			pointerPosition = oldPointerPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
		#else // For mobile
			if (Input.touchCount > 0)  
				pointerPosition = oldPointerPosition = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
		#endif


		return pointerPosition;
	}

    //-----------------------------------------------------------------------------------------------------	 
    // Get current rotation basing on mouse or touches
    float GetRotationDirection () 
	{
        float rotation = 0;

         // For Desktop - just set rotation to "clockwise" (don't change the permanent speed)
		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
			if (Input.GetMouseButton(1))
                rotation = 1;
        #else // For mobile - calculate angle changing between touches and use it.
            if (Input.touchCount > 1)  
            {
					// If there are two touches on the device... Store both touches.
					Touch touchZero = Input.GetTouch (0);
					Touch touchOne 	= Input.GetTouch (1);

					// Find the angle between positions.
					float currentAngle = Vector2.SignedAngle(touchZero.position, touchOne.position); 
					float previousAngle = Vector2.SignedAngle(touchZero.position - touchZero.deltaPosition, touchOne.position - touchOne.deltaPosition);

					rotation = currentAngle - previousAngle;
			}
                 //Alternative (sign/direction based):  // rotation = (int)Mathf.Sign(Vector2.SignedAngle(Vector2.up, Input.GetTouch(1).position-Input.GetTouch(0).position));
        #endif

        return rotation;
	}

	//-----------------------------------------------------------------------------------------------------	 
	// Switch puzzle and background to another
	public void SwitchPuzzle (PuzzleController _puzzle, Renderer _background)
	{
		if (_puzzle  &&  _puzzle != puzzle) 
			StartPuzzle (_puzzle);
		
		if (_background  &&  _background != background) 
			PrepareBackground (_background);
	}

	//-----------------------------------------------------------------------------------------------------	 
	// Prepare puzzle and Decompose it if needed
	public bool StartPuzzle (PuzzleController _puzzle)
	{
		if (!_puzzle) 
			_puzzle = gameObject.GetComponent<PuzzleController>();
		
		if (!_puzzle) 
		{
			Debug.LogWarning("PuzzleController should be assigned to puzzle property of GameController - check " + gameObject.name);  
			return false;
		}   


		if (puzzle  &&  puzzle.gameObject != gameObject) 
			puzzle.gameObject.SetActive(false);


		puzzle = _puzzle;
		puzzle.gameObject.SetActive(true); 
		
		Debug.Log(puzzle.puzzleBounds);


		if (puzzle.pieces == null) 
			puzzle.Prepare (); 

		if (!PlayerPrefs.HasKey (puzzle.name + "_Positions")  ||  !puzzle.enablePositionSaving)
			if (!invertRules) 
				puzzle.DecomposePuzzle (); 
			else
				puzzle.NonrandomPuzzle ();


		puzzle.invertedRules = invertRules;

		gameFinished = false;

		return true;
	}

	//-----------------------------------------------------------------------------------------------------	 
	// Show background (assembled puzzle)
	void ShowBackground () 
	{
		if (background  &&  backgroundColor.a < 1) 
		{
			backgroundColor.a = Mathf.Lerp (backgroundColor.a, 1.0f, Time.deltaTime); 
			background.material.color = backgroundColor;
		}

	}

	//-----------------------------------------------------------------------------------------------------	 
	// Prepare background (assembled puzzle)
	void PrepareBackground (Renderer _background) 
	{
		if (_background)
		{ 
			if (background)
				background.gameObject.SetActive(false);
			
			background = _background;
			background.gameObject.SetActive(true);

			/*backgroundColor = background.material.color;

			if (backgroundTransparency < 1.0f)
			{
				backgroundColor.a = backgroundTransparency; 
				background.material.color = backgroundColor;
			}*/
			//(background as SpriteRenderer).color = (_background as SpriteRenderer).color;
			background.material.color = _background.material.color;

			AdjustBackground();
		}
		else 
			background = null;

	}

	//-----------------------------------------------------------------------------------------------------	
	// Adjust background to puzzle
	void AdjustBackground () 
	{
		if (background  &&  background.transform.parent != puzzle.transform)  
		{
			background.transform.gameObject.SetActive(true);
			background.transform.parent = puzzle.transform;


			// Try to adjust background size according to puzzle bounds
			if (adjustBackground  &&  (background as SpriteRenderer).sprite)
			{
				// Temporarily reset Puzzle rotation 
				Quaternion tmpRotation = puzzle.transform.rotation;
				puzzle.transform.localRotation = Quaternion.identity;

				// Reset background transform
				background.transform.localPosition = new Vector3 (0, 0, 0.2f);
				background.transform.localRotation = Quaternion.identity;	
				background.transform.localScale = Vector3.one;

				// Calculate background scale  to make it the same size as puzzle
				background.transform.localScale = new Vector3(puzzle.puzzleDefaultBounds.size.x/background.bounds.size.x, puzzle.puzzleDefaultBounds.size.y/background.bounds.size.y, background.transform.localScale.z);	
				// Aligned background position
				background.transform.position = new Vector3(puzzle.puzzleDefaultBounds.min.x, puzzle.puzzleDefaultBounds.max.y, background.transform.position.z);


				// Shift background if it's origin not in LeftTop corner 		 			 	
				if (Mathf.Abs(background.bounds.min.x - puzzle.puzzleDefaultBounds.min.x) > 1  ||  Mathf.Abs(background.bounds.max.y - puzzle.puzzleDefaultBounds.max.y) > 1)
					background.transform.localPosition = new Vector3(background.transform.localPosition.x + background.bounds.extents.x,  background.transform.localPosition.y - background.bounds.extents.y,  background.transform.localPosition.z);

				// Return proprer puzzle rotation
				puzzle.transform.localRotation = tmpRotation;
			}
		}

	}

	//-----------------------------------------------------------------------------------------------------	 
	// Pause game and show pauseUI
	public void Pause ()
	{
		Time.timeScale = Time.timeScale > 0 ? 0 : 1;
	}


    //-----------------------------------------------------------------------------------------------------	 
    // Reset current puzzle
    public void ResetPuzzle()
    {
        if (puzzle == null)
            return;

        Time.timeScale = 0;

        puzzle.ResetProgress(puzzle.name);
        
        puzzle.DecomposePuzzle();      

        Time.timeScale = 1.0f;
    }

    //-----------------------------------------------------------------------------------------------------	 
    // Restart current level
    public void Restart () 
	{
		Time.timeScale = 1.0f;

		if (puzzle != null) 
		{
			PlayerPrefs.SetString (puzzle.name, "");
			PlayerPrefs.DeleteKey (puzzle.name + "_Positions");
		}

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}

	//-----------------------------------------------------------------------------------------------------	 
	// Load custom level
	public void LoadLevel (int _levelId) 
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene (_levelId);

	}
	

	//-----------------------------------------------------------------------------------------------------	
	// Save progress (Assembled pieces)
	public void Save ()
	{
		if (puzzle != null) 
		{
			puzzle.SaveProgress (puzzle.name);
		}

	}

	//-----------------------------------------------------------------------------------------------------	
	// Load puzzle (Assembled pieces)
	public void Load ()
	{
		if (!puzzle)
			return;
		else
			puzzle.LoadProgress(puzzle.name);
	}  

	//-----------------------------------------------------------------------------------------------------	
	// Save progress if player closes the application
	public void OnApplicationQuit() 
	{
		Save ();
		PlayerPrefs.Save();
	}
}