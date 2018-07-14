import React from "react";
import PropTypes from "prop-types";
import FlatButton from 'material-ui/FlatButton';

class Pagination extends React.PureComponent {
    constructor(props) {
        super(props);
        this.state = {
            currentPage: props.initialPage
        };

        this._onButtonClick = this._onButtonClick.bind(this);
    }

    componentWillReceiveProps(newProps) {
        if (newProps.initialPage !== this.props.initialPage) {
            this.setState({
                currentPage: newProps.initialPage
            });
        }
    }

    render() {
        let startPage = Math.max(1, this.state.currentPage - Math.trunc(this.props.showPagesCount / 2));
        startPage = Math.max(1, startPage + Math.min(0, this.props.pagesCount - (startPage + this.props.showPagesCount - 1)));
        const pagesButtons = [];
        for (let i = startPage; i < startPage + this.props.showPagesCount && i <= this.props.pagesCount; ++i) {
            pagesButtons.push(this._getButtonForPage(i, i, this.state.currentPage === i, {key: i}));
        } 

        return (
            <div>
                {this._getButtonForPage("<<", 1)}
                {this._getButtonForPage("<", this.state.currentPage - 1)}
                {pagesButtons}
                {this._getButtonForPage(">", this.state.currentPage + 1)}
                {this._getButtonForPage(">>", this.props.pagesCount)}
            </div>
        );
    }

    _onButtonClick(e) {
        const newPage = parseInt(e.currentTarget.dataset.page);
        if (this.state.currentPage !== newPage) {
            this.setState({
                currentPage: newPage
            });
            if (this.props.onPageChange) {
                this.props.onPageChange(newPage);
            }
        }
    }

    _getButtonForPage(label, page, selected = false, attributes = {}) {
        return <FlatButton label={label} primary={selected} onClick={this._onButtonClick} data-page={page} disabled={page === 0 || page > this.props.pagesCount} {...attributes} />
    }
}

Pagination.propTypes = {
    pagesCount: PropTypes.number.isRequired,
    initialPage: PropTypes.number,
    showPagesCount: PropTypes.number,
    onPageChange: PropTypes.func
};
Pagination.defaultProps = {
    initialPage: 1,
    showPagesCount: 5
};

export default Pagination;