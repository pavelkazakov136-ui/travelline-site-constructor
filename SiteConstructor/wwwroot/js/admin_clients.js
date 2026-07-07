async function loadClients() {
  const res = await fetch('/api/content');
  const data = await res.json();
  renderClientsCards(data.clients);
}

async function saveClientsOrder() {
    const cards = document.querySelectorAll('#client-cards .client-card');
    const orderedIds = Array.from(cards).map(c => Number(c.dataset.id));
    await fetch('/api/content/clients/reorder', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(orderedIds)
    });
}

function renderClientsCards(clients){
    const box = document.getElementById("client-cards");
    box.innerHTML = clients.map(c => `
        <div class="client-card" data-id="${c.id}">
            <small>id: ${c.id}</small>
            <input type="text" value="${c.logo}"     data-field="logo"     placeholder="логотип">
            <input type="text" value="${c.name}"     data-field="name"     placeholder="имя">
            <button type="button" data-action="save"   data-id="${c.id}">Сохранить</button>
            <button type="button" data-action="delete" data-id="${c.id}" class="secondary">✕</button>
        </div>`
    ).join('');
    Sortable.create(box, {
        animation: 150,
        onEnd: saveClientsOrder
    });
}

function collectClients(client){
    return{
        logo: client.querySelector('[data-field="logo"]').value, 
        name: client.querySelector('[data-field="name"]').value
    };
}

async function addClient(){
    const cards = document.querySelectorAll('#client-cards .client-card');
    const nextOrder = cards.length + 1;

    const res = await fetch('/api/content/clients',{
        method:'POST',
        headers:{'Content-Type': 'application/json'},
        body: JSON.stringify({
            logo: '', 
            name: 'Название',
            order: nextOrder
        })
    });
    if (res.ok) loadClients();
}

async function saveClient(id){
    const client = document.querySelector(`.client-card[data-id="${id}"]`)
    const res = await fetch(`/api/content/clients/${id}`,
        {
            method:'PUT',
            headers:{'content-Type': 'application/json'},
            body: JSON.stringify(collectClients(client))
        }
    );
    const msg = document.getElementById("client-msg");
    msg.textContent = res.ok ? 'Сохранено ✓' : 'Ошибка при сохранении';
}

async function deleteClient(id){
    const res = await fetch(`/api/content/clients/${id}`, {method:'DELETE'});
    if (res.ok) loadClients();
}

document.getElementById('client-cards').addEventListener('click',(event) =>{
    const action = event.target.dataset.action;
    const id = Number(event.target.dataset.id);
    if (action === 'save') saveClient(id);
    if (action === 'delete') deleteClient(id);
})

document.getElementById('add-client').addEventListener('click', addClient);

loadClients();
