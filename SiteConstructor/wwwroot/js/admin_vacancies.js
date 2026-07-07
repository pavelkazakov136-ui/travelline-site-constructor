async function loadVacancies(){
    const res = await fetch("/api/content");
    const data = await res.json();
    renderVacanci(data.vacancies);
}
async function saveVacanciesOrder() {
    const cards = document.querySelectorAll('#vacancy-cards .vacancy-card');
    const orderedIds = Array.from(cards).map(c => Number(c.dataset.id));
    await fetch('/api/content/vacancies/reorder', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(orderedIds)
    });
}

function renderVacanci(vacancies){
    const box = document.getElementById("vacancy-cards")
    box.innerHTML = vacancies.map(m =>`
        <div class="vacancy-card" data-id="${m.id}">
            <small>id: ${m.id}</small>
            <input type="text" value="${m.title}"     data-field="title"     placeholder="название">
            <input type="text" value="${m.format}" data-field="format" placeholder="формат">
            <input type="text" value="${m.url}"    data-field="url"    placeholder="ссылка">
            <button type="button" data-action="save"   data-id="${m.id}">Сохранить</button>
            <button type="button" data-action="delete" data-id="${m.id}" class="secondary">✕</button>
        </div>`
    ).join('');
    Sortable.create(box, {
        animation: 150,
        onEnd: saveVacanciesOrder
    });
}

function collectVacancy(vacancy){
    return{
        title: vacancy.querySelector('[data-field="title"]').value,
        format: vacancy.querySelector('[data-field="format"]').value,
        url: vacancy.querySelector('[data-field="url"]').value
    }
}

async function addVacancy(){
    const res = await fetch("/api/content/vacancies",{
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ title: 'Название', format: '', url: '' })
  });
  if (res.ok) loadVacancies();
}

async function saveVacancy(id) {
    const member = document.querySelector(`.vacancy-card[data-id="${id}"]`);
    const res = await fetch(`/api/content/vacancies/${id}`,
        {
            method:'PUT',
            headers:{'Content-Type':'application/json'},
            body: JSON.stringify(collectVacancy(member))
        }
    );
    const msg = document.getElementById("vacancy-msg");
    msg.textContent = res.ok ? 'Сохранено ✓' : 'Ошибка при сохранении';

}

async function deleteVacancy(id){
    const res = await fetch(`/api/content/vacancies/${id}`, {method: 'DELETE'});
    if (res.ok) loadVacancies();
}

document.getElementById("vacancy-cards").addEventListener('click',(event)=>{
    const action = event.target.dataset.action;
    const id = Number(event.target.dataset.id);
    if (action === 'save')   saveVacancy(id);
    if (action === 'delete') deleteVacancy(id);
})

document.getElementById("add-vacancy").addEventListener('click', addVacancy);

loadVacancies()