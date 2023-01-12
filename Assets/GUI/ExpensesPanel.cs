using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

public class ExpensesPanel : MonoBehaviour
{
    [SerializeField] bool isVertical = false;
    public GameObject build;
    [SerializeField] GameObject expense;
    [SerializeField] public List<Item> expenses = new List<Item>();
    [SerializeField] public List<int> counts= new List<int>();
    [SerializeField] int maxExpenseCount;
    int GridSpacing = 5;
    // Start is called before the first frame update
    void Start()
    {
        Rect Rect = GetComponent<RectTransform>().rect;
        if(isVertical)
        {
            GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            GetComponent<GridLayoutGroup>().constraintCount = 1;
            float cellSizeW = Rect.width - 6;
            float cellSizeH = (Rect.height - (maxExpenseCount * 3) - 3) /maxExpenseCount;
            GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSizeW,cellSizeH);
            GetComponent<GridLayoutGroup>().padding = new RectOffset(3, 3, 3, 3);

        }
        else
        {
            GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
            GetComponent<GridLayoutGroup>().constraintCount = 1;
            float expenseWidth = (Rect.width-(maxExpenseCount*3)-3) / (maxExpenseCount);
            float expenseHeight = Rect.height - 6;
            int paddingSize = (int)((GetComponent<RectTransform>().rect.width) - ((expenseWidth + GridSpacing) * expenses.Count - GridSpacing))/2 ;

            GetComponent<GridLayoutGroup>().cellSize = new Vector2(expenseWidth, expenseHeight);
            GetComponent<GridLayoutGroup>().padding = new RectOffset(paddingSize, paddingSize, 3, 3);
        }
            int i = 0; GameObject currentObject;
            foreach (var item in expenses)
            {
                currentObject = Instantiate(expense, transform);
                currentObject.GetComponentsInChildren<Image>()[1].sprite = item.icon;
                currentObject.GetComponentInChildren<Text>().text = counts[i++].ToString();
            }
               
    }

}
