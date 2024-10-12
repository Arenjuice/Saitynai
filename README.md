# Ūkio veiklų stebėjimo sistema

Sistemos paskirtis: registruoti atliktas veiklas laukui, kuris priklauso kažkokiam ūkiui. Ūkyje galima registruoti daug laukų. Kiekvienam laukui galima pildyti įrašus apie atliktas veiklas jame. Įrašas gali būti bet kokia veikla, kuri susijusiu su atitinkamu lauku.

Funkciniai reikalavimai: svečias gali peržiūrėti ūkių sąrašą bei susikurti ūkininko ar darbuotojo paskyrą. Darbuotojas gali būti pridėtas prie ūkio, būdamas ūkyje, jis gali pildyti įrašus apie atliktus darbus laukuose, gali matyti visus priskirto ūkio laukus ir įrašus apie juos. Ūkininkas gali kurti ūkį, pridėti į jį darbuotojus, pridėti laukus, pildyti įrašus apie atliktus darbus laukuose, taip pat mato visų ūkių sąrašą, bet pilną išklotinę mato tik savo ūkio.

Pasirinktos technologijos: ASP.net (backend), React.js (frontend), MSSQL Server (database).

Hierarchiniu ryšiu susiję taikomosios srities objektai: ūkis -> laukas -> įrašas.

Sistemos rolės: svečias, ūkininkas, darbininkas.
