using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cup : MonoBehaviour
{
    [SerializeField] private int cupIndex = 0;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material correctMaterial;
    [SerializeField] private Material incorrectMaterial;
    [SerializeField] private Transform lid;
    [SerializeField] private float liftHeight = 0.5f;
    [SerializeField] private float liftDuration = 0.3f;
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;

    private bool hasBall = false;
    private bool isLifted = false;
    private XRGrabInteractable grabInteractable;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;
    private Vector3 originalLidPosition;
    private Quaternion originalLidRotation;
    private GameObject ballInstance;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (ballSpawnPoint == null)
            ballSpawnPoint = transform;

        if (lid != null)
        {
            originalLidPosition = lid.localPosition;
            originalLidRotation = lid.localRotation;
        }

        // Subscribe na grab event
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnCupGrabbed);
        }
    }

    private void OnCupGrabbed(SelectEnterEventArgs args)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive())
            return;

        // Prikazi loptu i animiraj dizanje čaše
        ShowBall();
        PlaySound(selectSound);
        
        // Obavesti GameManager-a da je čaša odabrana
        GameManager.Instance.OnCupSelected(cupIndex);
    }

    public void SetIndex(int index)
    {
        cupIndex = index;
    }

    public void SetHasBall(bool value)
    {
        hasBall = value;
    }

    public bool HasBall()
    {
        return hasBall;
    }

    public void ShowBall()
    {
        if (!hasBall)
            return;

        // Kreiraj ball instancu ako ne postoji
        if (ballInstance == null)
        {
            ballInstance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ballInstance.transform.SetParent(ballSpawnPoint, false);
            ballInstance.transform.localPosition = Vector3.zero;
            ballInstance.transform.localScale = Vector3.one * 0.15f;
            
            // Ukloni collider sa balla
            Collider ballCollider = ballInstance.GetComponent<Collider>();
            if (ballCollider != null)
                Destroy(ballCollider);

            Material ballMat = new Material(Shader.Find("Standard"));
            ballMat.color = Color.red;
            ballInstance.GetComponent<MeshRenderer>().material = ballMat;
        }

        ballInstance.SetActive(true);
    }

    public void HideBall()
    {
        if (ballInstance != null)
        {
            ballInstance.SetActive(false);
        }
    }

    public void Highlight(bool correct)
    {
        if (correct)
        {
            meshRenderer.material = correctMaterial;
            PlaySound(correctSound);
        }
        else
        {
            meshRenderer.material = incorrectMaterial;
            PlaySound(incorrectSound);
        }

        // Vrati normalnu boju posle 1 sekunde
        Invoke(nameof(ResetMaterial), 1f);
    }

    public void ResetMaterial()
    {
        meshRenderer.material = normalMaterial;
    }

    public void EnableInteraction()
    {
        if (grabInteractable != null)
            grabInteractable.enabled = true;
    }

    public void DisableInteraction()
    {
        if (grabInteractable != null)
            grabInteractable.enabled = false;
    }

    public void Reset()
    {
        HideBall();
        ResetMaterial();
        isLifted = false;

        if (lid != null)
        {
            lid.localPosition = originalLidPosition;
            lid.localRotation = originalLidRotation;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnCupGrabbed);
        }

        if (ballInstance != null)
        {
            Destroy(ballInstance);
        }
    }
}
