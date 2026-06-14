# Cups and Balls VR - BRZO POČETAK

## 🚀 KORAK 1: Kloniraj Repo

```bash
git clone https://github.com/radovanovicema/cups-and-balls-vr.git
cd cups-and-balls-vr
```

## 🛠️ KORAK 2: Otvori u Unity 6

1. Otvori **Unity Hub**
2. Klikni **Open Project**
3. Odaberi folder `cups-and-balls-vr`
4. Čekaj da se projekat učita (~2 minuta)

## ⚠️ KORAK 3: Instaliraj XR Toolkit (Prvi Put)

Kada se Unity učita:

```
1. Package Manager → + (Add package)
2. Add package by name → com.unity.xr.interaction.toolkit → Add
3. Čekaj da se instalira
4. Window → XR → Samples → Install XR Device Simulator
```

## ✅ KORAK 4: Kreiraj Scene

### OPCIJA A: Auto-Generate (Preporučeno)
```
1. Desni klik u Hierarchy
2. Create Empty → GameObject
3. Dodaj SceneGenerator.cs skriptu na taj GameObject
4. Uradi: SceneGenerator.GenerateScene()
5. Gotovo! ✨
```

### OPCIJA B: Ručno (5 minuta)
Vidi MANUAL_SETUP.md

## 🎮 KORAK 5: Pokreni!

```
1. File → Save Scene As → Assets/Scenes/CupsAndBalls.unity
2. Pritiski Play ▶️ u editoru
3. Igra počinje!
```

---

## 🎯 Test Igru

| Akcija | Šta se dešava |
|--------|--------------|
| Play | Igra počinje, vidiš 2 crvene kuglice |
| Čekaj 3 sec | Kuglice nestaju, čaše se mešaju |
| Desni klik + drag na čašu | Grabovanje čaše, pronalaženje loptice |
| Pronađi obe loptice | 🎉 Pobeda! |

---

## 📞 Problemi?

**Ako nema XR Toolkit-a:**
```
Package Manager → com.unity.xr.interaction.toolkit
```

**Ako scene ne radi:**
1. Proverite Console za greške
2. Vidite SETUP.md za detaljno uputstvo

**Ako se čaše ne grabuju:**
1. Proverite XR Device Simulator je instaliran
2. Proverite XRGrabInteractable na svakoj čašu

---

**Sretno!** 🚀
