async function loadBonuses() {
  const res = await fetch('/api/content');
  const data = await res.json();
  renderBonusesCards(data.bonuses);
}

function renderBonusesCards(bonuses){
    const box = document.getElementById("bonuses-cards");
    box.innerHTML = bonuses.map(b => `
        <div class="bonus-card" data-id="${b.id}">
            <small>id: ${b.id}</small>
            <input type="text" value="${b.title}"     data-field="title"     placeholder="Заголовок">
            <input type="text" value="${b.subtitle}"     data-field="subtitle"     placeholder="Описание">
            <button type="button" data-action="save"   data-id="${b.id}">Сохранить</button>
            <button type="button" data-action="delete" data-id="${b.id}" class="secondary">✕</button>
        </div>`
    ).join('');
}

function collectBonuses(bonus){
    return{
        title: bonus.querySelector('[data-field="title"]').value, 
        subtitle: bonus.querySelector('[data-field="subtitle"]').value
    };
}

async function addBonus(){
    const res = await fetch('/api/content/bonuses',{
        method:'POST',
        headers:{'content-Type': 'application/json'},
        body: JSON.stringify({title: '', subtitle: ''})
    });
    if (res.ok) loadBonuses()
}

async function saveBonus(id){
    const bonus = document.querySelector(`.bonus-card[data-id="${id}"]`)
    const res = await fetch(`/api/content/bonuses/${id}`,
        {
            method:'PUT',
            headers:{'content-Type': 'application/json'},
            body: JSON.stringify(collectBonuses(bonus))
        }
    );
    const msg = document.getElementById("bonus-msg");
    msg.textContent = res.ok ? 'Сохранено ✓' : 'Ошибка при сохранении';
}

async function deleteBonus(id){
    const res = await fetch(`/api/content/bonuses/${id}`, {method:'DELETE'});
    if (res.ok) loadBonuses();
}

document.getElementById('bonuses-cards').addEventListener('click',(event) => {
    const action = event.target.dataset.action;
    const id = Number(event.target.dataset.id);
    if (action === 'save') saveBonus(id);
    if (action === 'delete') deleteBonus(id);
})

document.getElementById('add-bonus').addEventListener('click', addBonus);

loadBonuses();
