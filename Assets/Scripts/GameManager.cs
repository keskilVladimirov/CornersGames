using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private PieceManager pieceManager;
    
    private bool PvP = true;
    private string gameMode;
    
    private void Start()
    {
        gameMode = PlayerPrefs.GetString("gameMode");
        
        //TODO: AI 
        //PvP = PlayerPrefs.GetInt("pvp") == 0 ? true : false;
        
        board.Create();
        pieceManager.Setup(board, gameMode, PvP);
    }
}
