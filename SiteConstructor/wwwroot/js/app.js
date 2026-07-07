async function loadContent() { 
  const res = await fetch('/api/content'); 
  const data = await res.json(); 
  
  renderHero(data.hero);
  renderTeam(data.team);
  renderClients(data.clients);
  renderBonus(data.bonuses); 
  renderForm(data.form); 
  renderVacancies(data.vacancies);
  renderGallery(data.gallery);
  setupStatsScroll();
}

function renderHero(hero) {
  document.querySelector('.hero__title').innerHTML = hero.subtitle.replaceAll('\n', '<br>');
  const stats__list = document.querySelector('.stats__list');
  let htmlContent = hero.stats.map(stat => {
    const cleanLabel = stat.label.replaceAll('\n', '<br>')
    return `<li class="stats__item" data-value="${stat.value}">${cleanLabel}</li>`}).join('');
  stats__list.innerHTML = htmlContent;
}

function renderStatValue(value) {
  return value.endsWith('.svg')
    ? `<img class="stats__value-logo" src="/images/${value}" alt="TravelLine">`
    : value;
}

function setupStatsScroll() {
  const valueEl = document.querySelector('.stats__value');
  const items = document.querySelectorAll('.stats__item');

  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        valueEl.innerHTML = renderStatValue(entry.target.dataset.value);
        items.forEach(i => i.classList.remove('stats__item--active'));
        entry.target.classList.add('stats__item--active');
      }
    });
  }, {
    root: null,                      
    rootMargin: '-50% 0px -50% 0px', 
    threshold: 0
  });

  items.forEach(item => observer.observe(item));
}

function renderTeam(team) {
  const team__track = document.querySelector('.team__track')
  let htmlContent = team.map(personality => 
    `<li class="team__card">
          <img class="team__photo" src="/images/${personality.photo}" alt="${personality.name}" />
          <div class="team__meta">
            <span class="team__name">${personality.name}</span>
            <span class="team__role">${personality.position}</span>
          </div>
        </li>`
  ).join('');
  team__track.innerHTML = htmlContent; 
}

function renderVacancies(vacancies){
  const vacancies__list = document.querySelector('.vacancies__list');
  let htmlContent = vacancies.map(vacancy => 
    `<li class="vacancies__item">
          <h3 class="vacancies__item-title">${vacancy.title}</h3>
          <div class="vacancies__item-footer">
            <p class="vacancies__item-place">${vacancy.format}</p>
            <img class="vacancies__item-icon" src="images/hhru.svg" alt="hh.ru" />
          </div>
          <a class="vacancies__item-link" href="${vacancy.url}" target="_blank" rel="noopener" aria-label="${vacancy.title}"></a>
        </li>`
  ).join('');
  htmlContent += 
        `<li class="vacancies__item vacancies__item--more">
          <h3 class="vacancies__item-title">Еще больше вакансий на HeadHunter →</h3>
          <a class="vacancies__item-link" href="https://yoshkar-ola.hh.ru/search/vacancy?from=employerPage&amp;hhtmFrom=employer&amp;employer_id=1136961" target="_blank" rel="noopener" aria-label="Все вакансии на HeadHunter"></a>
        </li>`
  vacancies__list.innerHTML = htmlContent;

}

function renderClients(clients) {
  const track = document.querySelector('.clients__track');
  const renderItem = (client, isDuplicate = false) => `
    <li class="clients__item" ${isDuplicate ? 'aria-hidden="true"' : ''}>
      <img class="clients__logo" src="/images/${client.logo}" alt="${client.name}" />
    </li>`;

  const original  = clients.map(c => renderItem(c)).join('');
  const duplicate = clients.map(c => renderItem(c, true)).join('');

  track.innerHTML = original + duplicate;
}

function renderGallery(gallery) {
  const grid = document.querySelector('.gallery__grid');

  const cols = [[], [], []];
  gallery.forEach((item, i) => cols[i % 3].push(item));

  const renderMedia = (item) => {
    const isVideo = item.media.endsWith('.mp4');
    return isVideo
      ? `<video class="gallery__media" src="/images/${item.media}" autoplay muted loop playsinline></video>`
      : `<img class="gallery__media" src="/images/${item.media}" alt="" loading="lazy">`;
  };

  grid.innerHTML = cols.map(col => `
    <div class="gallery__col">
      ${col.map(item => `
        <div class="gallery__item">
          ${renderMedia(item)}
          <p class="gallery__caption">${item.title.replaceAll('\n', '<br>')}</p>
        </div>
      `).join('')}
    </div>
  `).join('');
}

function renderBonus(bonuses) {
  const grid = document.querySelector('.bonus__grid');
  const colors = ['#8b5cf6', '#22a55c', '#f97316', '#3b82f6', '#ec4899', '#06b6d4'];
  const cols = [[], [], []];
  bonuses.forEach((item, i) => cols[i % 3].push({ ...item, color: colors[i % colors.length] }));

  grid.innerHTML = cols.map(col => `
    <div class="bonus__col">
      ${col.map(item => `
        <div class="bonus__card">
          <h3 class="bonus__card-title" style="color:${item.color}">${item.title}</h3>
          <p class="bonus__card-text">${item.subtitle}</p>
        </div>
      `).join('')}
    </div>
  `).join('');
}

function renderForm(form) {
  document.querySelector('.contact__title').textContent = form.title;
  document.querySelector('.contact__subtitle').textContent = form.subtitle;
  document.querySelector('.contact__submit').textContent = form.button;
}

document.querySelector('.contact__form').addEventListener('submit', async (event) => {
    event.preventDefault();  

    const form = event.target;
    const inputs = form.querySelectorAll('.contact__input');
    const submission = {
        name: inputs[0].value,
        phone: inputs[1].value,
        email: inputs[2].value,
        resume: inputs[3].value,
        direction: inputs[4].value  
    };

    const res = await fetch('/api/submissions', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(submission)
    });

    if (res.ok) {
        form.reset();                        
        alert('Спасибо! Заявка отправлена.'); 
    } else {
        alert('Ошибка отправки. Проверьте поля.');
    }
});

loadContent();   