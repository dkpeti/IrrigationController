# <img src="https://github.com/user-attachments/assets/14c80afa-7b0e-418a-88bb-c34fc31a8360" alt="Logo" style="height: 1em; vertical-align: middle;" /> Irrigation Controller - Android App



Az **IrrigationController** egy öntözőrendszer vezérlő alkalmazás, amely **Visual Studio C#** és **Xamarin** keretrendszerrel fejlesztett alkalmazás Androidra. A projekt célja, hogy a felhasználók távolról is könnyedén kezelhessék otthoni öntözőrendszerüket, figyelembe véve a helyi időjárási viszonyokat és szenzoradatokat.

## Főbb jellemzők
- **Google-fiókkal való bejelentkezés** – Biztonságos hozzáférés a személyes adatokhoz.  
- **Raspberry Pi-alapú adatgyűjtés** – Valós idejű mérések talajnedvességről és hőmérsékletről.  
- **Többzónás kezelés** – Különböző kerti zónák öntözésének egyedi szabályozása.  
- **Időzített öntözés** – Testreszabható időtartamú működtetés.  
- **Két szerveres architektúra** – Két külön szerver ([IrrigationServer](https://github.com/dkpeti/IrrigationServer) és [PiServer](https://github.com/dkpeti/PiServer)) biztosítja az adatkezelést és kommunikációt.  

## Felépítés és működés

### Szerveroldal
1. **IrrigationServer** (.NET Core)  
   - Felhasználói adatok és konfigurációk tárolása (PostgreSQL adatbázis).  
   - API-t biztosít a mobilalkalmazás számára.  
   - Kommunikál a `PiServer`-rel a szenzoradatok fogadásához.  

2. **PiServer** (.NET Core, Raspberry Pi-n futtatható)  
   - Szenzorok adatainak gyűjtése
   - Kommunikáció a főszerverrel

### Mobilalkalmazás (Xamarin)  
- **Felhasználói felület**: Letisztult, Material Design alapú elrendezés.  
- **Funkciók**:  
  - Pi hozzáadása/szerkesztése/törlése.  
  - A Pi-hez tartozó szenzorok megjelenítése
  - A szenzorok mérési előzményei megjelenítése
  - A szenzorok szerkesztése/törlése
  - Öntözési zóna hozzáadása/szerkesztése/törlése
  - Öntözés manuális indítása/leállítása
  - Hiba és Exception kezelés

## Képernyőképek

### Bejelentkezés
<img src="https://github.com/user-attachments/assets/d96072b9-6989-44ee-8cb2-03724ea3fdc8" alt="Bejelentkezés" width="260px" title="Bejelentkezési képernyő"/>

### Pi kezelése
<div style="display: flex; flex-wrap: wrap; gap: 15px; margin-bottom: 20px;">
  <img src="https://github.com/user-attachments/assets/ca567ab1-1757-40e1-a8bc-9fd386cb8fc5" alt="piAdd" width="260px"/>
  <img src="https://github.com/user-attachments/assets/8f88ec15-38a8-4eb4-94fa-3ccc89dd1673" alt="piAll" width="260px"/>
  <img src="https://github.com/user-attachments/assets/5d0ad9e4-2b64-4c49-a384-ce103229e3b1" alt="piData" width="260px"/>
</div>

### Szenzorok
<div style="display: flex; flex-wrap: wrap; gap: 15px; margin-bottom: 20px;">
  <img src="https://github.com/user-attachments/assets/b8c55132-5ffe-4268-8d76-d98e07b30c6a" alt="sensorData" width="260px"/>
  <img src="https://github.com/user-attachments/assets/cee2a446-5e1b-45bc-9cd6-034e9ee85969" alt="sensorData" width="260px"/>
</div>

### Zóna kezelése
<div style="display: flex; flex-wrap: wrap; gap: 15px; margin-bottom: 20px;">
  <img src="https://github.com/user-attachments/assets/a6612352-2e49-433f-b595-0dbc5e669d70" alt="zonaAdd" width="260px"/>
  <img src="https://github.com/user-attachments/assets/7135b152-c856-4082-abc5-226eb18f2663" alt="zonaAll" width="260px"/>
  <img src="https://github.com/user-attachments/assets/e85d3ce1-a63f-49b7-af89-2ec25d685e42" alt="zonaDelete" width="260px"/>
</div>

### Öntözés kezelése
<img src="https://github.com/user-attachments/assets/b706739f-5d09-4ec5-bd61-62c11b15faf7" alt="Öntözés indítása" width="780px"/>
