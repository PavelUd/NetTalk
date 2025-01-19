import AbstractView from '../framework/view/abstract-view.js';

const createErrorTemplate = () => `<div class="d-flex justify-content-center">
            <div class="" role="status">
                <span class="">Failed to load chat</span>
            </div>
        </div>`;

export default class ErrorView extends AbstractView {
    get template() {
        return createErrorTemplate();
    }
}