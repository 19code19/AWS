import React, { useEffect } from 'react';
import ReactDOM from 'react-dom';
import { createMemoryHistory } from 'history';
import useBlockHistoryPop from './useBlockHistoryPop';

// A helper component to use the hook in a testing scenario
const TestComponent = ({ history }) => {
  useBlockHistoryPop(history);
  return null;
};

describe('useBlockHistoryPop', () => {
  let history;
  let unblockMock;
  let container;

  beforeEach(() => {
    history = createMemoryHistory();
    unblockMock = jest.fn();
    history.block = jest.fn().mockReturnValue(unblockMock);

    // Set up a DOM container for rendering
    container = document.createElement('div');
    document.body.appendChild(container);
  });

  afterEach(() => {
    // Clean up the DOM container after each test
    ReactDOM.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  it('should block history on POP action', () => {
    ReactDOM.render(<TestComponent history={history} />, container);

    // Expect history.block to be called with a function
    expect(history.block).toHaveBeenCalledWith(expect.any(Function));
  });

  it('should unblock when the component is unmounted', () => {
    ReactDOM.render(<TestComponent history={history} />, container);

    // Unmount the component and check unblock is called
    ReactDOM.unmountComponentAtNode(container);
    expect(unblockMock).toHaveBeenCalled();
  });

  it('should return false when action is POP', () => {
    ReactDOM.render(<TestComponent history={history} />, container);

    const blockerFunction = history.block.mock.calls[0][0];
    const result = blockerFunction('/somepath', 'POP');
    
    expect(result).toBe(false);
  });

  it('should allow other actions', () => {
    ReactDOM.render(<TestComponent history={history} />, container);

    const blockerFunction = history.block.mock.calls[0][0];
    const result = blockerFunction('/somepath', 'PUSH');

    expect(result).toBeUndefined();  // Undefined means no block.
  });
});
