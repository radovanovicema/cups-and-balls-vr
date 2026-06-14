# Scripts Structure

## Core Scripts

### GameManager.cs
- Upravljač glavne logike igre
- Kontroliše sve faze: Showing → Shuffling → Playing → GameOver
- Prosledi događaje kada korisnik odabere čašu
- Sadrži maxAttempts (3) i score tracking

### CupManager.cs
- Upravljač svim čašama (5 komada)
- Logika mešanja čaša (swap animacija)
- Prikaži/sakrij lopte
- Enable/disable interakcije

### Cup.cs
- Individualna čaša logika
- XR Grab Interactable integracija
- Ball spawning
- Highlight animacije (zelena/crvena)
- Audio feedback

## UI Scripts

### UIManager.cs
- Prikaz poruka korisniku
- Prikazivanje preostalih pokušaja
- Restart dugme
- Audio feedback za UI

## How to Use

1. Kreiraj 5 čaša u sceni (cylinder ili custom model)
2. Dodaj Cup.cs skriptu na svaku čašu
3. Dodaj XRGrabInteractable komponentu
4. Dodaj GameManager sa CupManager
5. Dodaj UIManager sa Canvas-om
6. Poveži reference u Inspector-u

## Key Features

- ✅ 5 čaša, 2 nasumične loptice
- ✅ Prikaži faza (3 sekunde)
- ✅ Mešanje faza (5 sekundi sa animacijama)
- ✅ Igranje faza (korisnik grabuje čaše)
- ✅ Max 3 pokušaja
- ✅ Score tracking i GameOver logika
- ✅ XR Input handling
- ✅ Audio i visual feedback
