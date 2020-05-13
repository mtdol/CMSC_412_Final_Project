using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DungeonStatusTextController : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        this.text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // sets the text text1 for the given amount of time before switching to text2
    public IEnumerator SetTextTemporary(string text1, string text2, float time)
    {
        // set the initial text
        this.text.text = text1;

        // now wait
        yield return new WaitForSeconds(time);
        
        // now set the second text
        this.text.text = text2;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
