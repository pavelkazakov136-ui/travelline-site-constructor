async function loadTeam() {
  const res = await fetch('/api/content');
  const data = await res.json();
  renderTeamCards(data.team);
}

async function saveTeamOrder() {
    const cards = document.querySelectorAll('#team-cards .team-card');
    const orderedIds = Array.from(cards).map(card => Number(card.dataset.id));

    await fetch('/api/content/team/reorder', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(orderedIds)
    });
}

function renderTeamCards(team) {
  const box = document.getElementById('team-cards');
  box.innerHTML = team.map(m => `
    <div class="team-card" data-id="${m.id}">
      <small>id: ${m.id}</small>
      <input type="text" value="${m.name}"     data-field="name"     placeholder="имя">
      <input type="text" value="${m.position}" data-field="position" placeholder="должность">
      <input type="text" value="${m.photo}"    data-field="photo"    placeholder="путь к фото">
      <button type="button" data-action="save"   data-id="${m.id}">Сохранить</button>
      <button type="button" data-action="delete" data-id="${m.id}" class="secondary">✕</button>
    </div>
  `).join('');
  Sortable.create(box, {
    animation: 150,
    onEnd: saveTeamOrder
  });
}

function collectCard(card){
    return {
        name: card.querySelector('[data-field="name"]').value,
        position: card.querySelector('[data-field="position"]').value,
        photo: card.querySelector('[data-field="photo"]').value
    };
}


async function addMember() {
  const res = await fetch('/api/content/team', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name: 'Имя', position: '', photo: '' })
  });
  if (res.ok) loadTeam();  
}

async function saveMember(id) {
    const member = document.querySelector(`.team-card[data-id="${id}"]`);
    const res = await fetch(`/api/content/team/${id}`,
        {
            method:'PUT',
            headers:{'Content-Type':'application/json'},
            body: JSON.stringify(collectCard(member))
        }
    );
    const msg = document.getElementById("team-msg");
    msg.textContent = res.ok ? 'Сохранено ✓' : 'Ошибка при сохранении';

}

async function deleteMember(id) {
  const res = await fetch(`/api/content/team/${id}`, { method: 'DELETE' });
  if (res.ok) loadTeam();
}

document.getElementById('team-cards').addEventListener('click', (event) => {
  const action = event.target.dataset.action;
  const id = Number(event.target.dataset.id);
  if (action === 'save')   saveMember(id);
  if (action === 'delete') deleteMember(id);
});

document.getElementById('add-member').addEventListener('click', addMember);

loadTeam();