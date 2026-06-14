# Cups and Balls VR - Arhitektura Projekta

## Game Flow Dijagram

```
START
  ↓
GameManager.StartGame()
  ├─ Randomize ball positions (2 od 5)
  ├─ CupManager.SetBallPositions()
  └─ ShowPhase()
      ├─ CupManager.ShowBalls() [3 sekunde]
      ├─ CupManager.HideBalls()
      └─ ShufflePhase()
          ├─ CupManager.ShuffleCups() [5 sekundi]
          │   └─ SwapCups() x N puta
          └─ PlayPhase()
              ├─ cupManager.EnableAllCups()
              └─ Player grabuje čaše
                  ├─ OnCupSelected() → Cup.ShowBall()
                  ├─ Ako ima loptu → ✓ (zelena)
                  ├─ Ako nema → ✗ (crvena)
                  └─ correctGuesses++
                      ├─ Ako == 2 → WIN
                      └─ Ako attemppts == 3 → LOSE
  ↓
GameOverPhase()
  ├─ Prikaži rezultat
  ├─ Restart dugme
  └─ [RESTART] → StartGame()
```

---

## Class Relationships

```
GameManager (Singleton)
├── CupManager
│   └── Cup[] (5 komada)
│       ├── XRGrabInteractable
│       ├── Ball (spawned at runtime)
│       └── MeshRenderer (za highlight)
└── UIManager
    ├── Canvas
    │   ├── messageText (TextMeshPro)
    │   ├── attemptsText (TextMeshPro)
    │   └── restartButton (Button)
    └── AudioSource
```

---

## Script Odgovornosti

### GameManager.cs
**Glavna logika igre**
- State machine (Menu → Showing → Shuffling → Playing → GameOver)
- Random ball placement
- Attempt tracking
- Game flow orchestration
- Score management

### CupManager.cs
**Upravljanje čašama**
- Cup indexing i positioning
- Shuffle algorithm (nasumične zamene)
- Show/Hide balls
- Enable/Disable cup interactions
- Batch operations na čašama

### Cup.cs
**Individualna čaša**
- XR Grab handling
- Ball spawning/destroying
- Visual feedback (highlight)
- Audio feedback
- State management (lifted, selected, itd.)

### UIManager.cs
**Korisnički interfejs**
- Text message display
- Attempts counter
- Restart button handling
- UI audio feedback
- Canvas management

### GameSetup.cs
**Inicijalizacija**
- Auto-detection svih komponenti
- Validation i error checking
- Debug logging

---

## Data Flow

```
GameManager startuje
    ↓
Bira 2 nasumične čaše za loptice
    ↓
CupManager.SetBallPositions(list)
    ↓
Svaka Cup komponenta zna da li ima loptu
    ↓
CupManager.ShowBalls() → Ball objects spawn
    ↓
CupManager.ShuffleCups() → Animacija zamene
    ↓
Čaše se fizički pomeren ali logika se čuva
    ↓
Player grabuje čašu → Cup.OnCupGrabbed()
    ↓
Cup.HasBall() → GameManager.OnCupSelected()
    ↓
Ako tačno → correctGuesses++
    ↓
Ako 2 tačne → WIN else ako 3 pokušaja → LOSE
```

---

## XR Integration

### XR Origin Setup
- Kamera je dete od XROrigin
- XR Controllers dolaze kao deca
- Input je automatski mapiran

### Grabovanje
- XRGrabInteractable component
- selectEntered event na Cup.cs
- Cup reaguje kada se grabuje

### Simulacija
- XR Device Simulator (iz Package Manager)
- Leva miš = Levo oko
- Desna miš = Desna ruka grabovanje

---

## Performance Considerations

### Optimizacije:
- Ball objects se kreiraju samo kada trebaju
- Coroutines za sve animacije (ne ažurira svaki frame)
- List<int> za ball positions umesto individual checks
- Single GameManager (Singleton pattern)
- Object pooling za balls (future improvement)

### Skalabilnost:
- Lako se mogu dodati nove čaše
- UI je decoupled od game logic
- Audio i visual feedback su optional
- Mogućnost za različite difficulty levels

---

## Future Enhancements

- [ ] Difficulty levels (brže mešanje, više pokušaja)
- [ ] Leaderboard system
- [ ] Multiplayer mode
- [ ] Hand tracking support
- [ ] Custom cup models
- [ ] Sound effects library
- [ ] Particle effects
- [ ] Haptic feedback

---

## Debugging Tips

**Ako igra ne radi:**

1. Proverite Console za error messages
2. Proverite da sve scene reference su ispunjene
3. Proverite XR Plugin Management settings
4. Proverite da XR Device Simulator je instaliran

**Debug Mode:**
```csharp
// Dodaj u GameManager.cs Start() za ispis
Debug.Log("Ball positions: " + string.Join(", ", ballPositions));
Debug.Log("Game state: " + currentState);
```

---

**Dokumentacija**: Created by Ema Radovanović - 2026
