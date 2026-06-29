let heroData;

async function loadHero(){
    const res = await fetch('/api/content');
    const data = await res.json();
    heroData = data.hero;
    document.getElementById("hero-subtitle").value = heroData.subtitle;
    renderStatsInputs(heroData.stats);
}

function renderStatsInputs(stats){
    const box = document.getElementById("hero-stats");
    box.innerHTML = stats.map((stat, i) =>`
    <div class="inputs-field">
      <input type="text" value="${stat.value}" data-field="value" data-index="${i}" placeholder="value">
      <input type="text" value="${stat.label}" data-field="label" data-index="${i}" placeholder="label">
      <button type="button" data-remove="${i}" class="secondary">✕</button>
    </div>
  `).join('');
}

function collectStats(){
    const inputs = document.querySelectorAll(".inputs-field");
    return Array.from(inputs).map(input => {
        return{
            value: input.querySelector('[data-field="value"]').value,
            label: input.querySelector('[data-field="label"]').value
        }; 
    });
}

async function saveHero(){
    const updatedHero = {
        subtitle: document.getElementById("hero-subtitle").value,
        stats:collectStats()
    }
    const res = await fetch('/api/content/hero',{
        method: 'PUT',
        headers:{'Content-Type': 'application/json'},
        body: JSON.stringify(updatedHero)
    });
    const msg = document.getElementById("hero-msg");
    msg.textContent = res.ok ? 'Сохранено ✓' : 'Ошибка при сохранении';
}

document.getElementById("save-hero").addEventListener('click', saveHero);

document.getElementById("add-stat").addEventListener('click',() => {
    heroData.stats = collectStats();
    heroData.stats.push({value:'', label:''});
    renderStatsInputs(heroData.stats);
});

document.getElementById("hero-stats").addEventListener('click', (event)=>{
    if(event.target.dataset.remove !== undefined){
        heroData.stats = collectStats();
        heroData.stats.splice(Number(event.target.dataset.remove), 1);
        renderStatsInputs(heroData.stats);
    }
});

loadHero();