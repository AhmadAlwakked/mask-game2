using UnityEngine;

public class PianoObject : MonoBehaviour
{
    [Header("Piano Settings")]
    public GameObject pianoModel; // Parent object van piano

    private AudioSource pianoAudio; // AudioSource op piano
    private bool isPlaying = true;  // Staat het geluid aan?

    private Outline[] outlines;     // Alle Outline componenten op piano en children

    void Start()
    {
        if (pianoModel == null)
            pianoModel = this.gameObject;

        // Haal AudioSource op van het piano object
        pianoAudio = GetComponent<AudioSource>();
        if (pianoAudio == null)
        {
            Debug.LogWarning("Geen AudioSource gevonden op piano object!");
            isPlaying = false;
        }
        else
        {
            pianoAudio.loop = true;
            pianoAudio.playOnAwake = true;
            pianoAudio.Play();
            isPlaying = true;
        }

        // Voeg Outline toe aan alle children met MeshRenderer
        var meshRenderers = pianoModel.GetComponentsInChildren<MeshRenderer>();
        outlines = new Outline[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            Outline o = meshRenderers[i].GetComponent<Outline>();
            if (o == null)
                o = meshRenderers[i].gameObject.AddComponent<Outline>();

            outlines[i] = o;

            // Zorg dat QuickOutline instance klaar is
            o.enabled = true;
            o.OutlineWidth = 0f; // start zonder highlight
        }
    }

    /// <summary>
    /// Zet highlight aan/uit op alle Outline componenten van de piano en children
    /// </summary>
    public void Highlight(bool value)
    {
        if (outlines == null) return;

        foreach (Outline o in outlines)
        {
            if (o != null)
            {
                o.enabled = true;
                o.OutlineWidth = value ? 5f : 0f;
            }
        }
    }

    /// <summary>
    /// Toggle het pianogeluid aan/uit
    /// </summary>
    public void TogglePiano()
    {
        if (pianoAudio == null) return;

        if (isPlaying)
        {
            pianoAudio.Pause();
            isPlaying = false;
        }
        else
        {
            pianoAudio.UnPause();
            isPlaying = true;
        }
    }
}
