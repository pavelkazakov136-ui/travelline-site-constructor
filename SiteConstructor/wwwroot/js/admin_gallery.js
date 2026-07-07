async function loadGallery() {
  const res = await fetch('/api/content');
  const data = await res.json();
  renderGalleryCards(data.gallery);
}
async function saveGalleryOrder() {
    const cards = document.querySelectorAll('#gallery-cards .gallery-card');
    const orderedIds = Array.from(cards).map(c => Number(c.dataset.id));
    await fetch('/api/content/gallery/reorder', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(orderedIds)
    });
}
function renderGalleryCards(gallery){
    const box = document.getElementById("gallery-cards");
    box.innerHTML = gallery.map(g => `
        <div class="gallery-card" data-id="${g.id}">
            <small>id: ${g.id}</small>
            <input type="text" value="${g.title}"     data-field="title"     placeholder="текст">
            <input type="text" value="${g.media}"     data-field="media"     placeholder="Путь">
            <button type="button" data-action="save"   data-id="${g.id}">Сохранить</button>
            <button type="button" data-action="delete" data-id="${g.id}" class="secondary">✕</button>
        </div>`
    ).join('');
    Sortable.create(box, {
        animation: 150,
        onEnd: saveGalleryOrder
    });
}

function collectGallery(gallery){
    return{
        title: gallery.querySelector('[data-field="title"]').value, 
        media: gallery.querySelector('[data-field="media"]').value
    };
}

async function addGallery(){
    const res = await fetch('/api/content/gallery',{
        method:'POST',
        headers:{'content-Type': 'application/json'},
        body: JSON.stringify({title: 'Текс', media: ''})
    });
    if (res.ok) loadGallery()
}

async function saveGallery(id){
    const gallery = document.querySelector(`.gallery-card[data-id="${id}"]`)
    const res = await fetch(`/api/content/gallery/${id}`,
        {
            method:'PUT',
            headers:{'content-Type': 'application/json'},
            body: JSON.stringify(collectGallery(gallery))
        }
    );
    const msg = document.getElementById("gallery-msg");
    msg.textContent = res.ok ? 'Сохранено ✓' : 'Ошибка при сохранении';
}

async function deleteGallery(id){
    const res = await fetch(`/api/content/gallery/${id}`, {method:'DELETE'});
    if (res.ok) loadGallery();
}

document.getElementById('gallery-cards').addEventListener('click',(event) =>{
    const action = event.target.dataset.action;
    const id = Number(event.target.dataset.id);
    if (action === 'save') saveGallery(id);
    if (action === 'delete') deleteGallery(id);
})

document.getElementById('add-gallery-item').addEventListener('click', addGallery);

loadGallery();
